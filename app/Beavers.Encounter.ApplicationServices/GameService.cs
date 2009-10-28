﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Beavers.Encounter.ApplicationServices.Utils;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using ICSharpCode.SharpZipLib.Zip;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.ApplicationServices
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> gameRepository;
        private readonly IRepository<Team> teamRepository;
        private readonly IRepository<TeamGameState> teamGameStateRepository;
        private readonly ITaskService taskService;

        private GameDemon gameDemon;

        public GameService(
            IRepository<Game> gameRepository,
            IRepository<Team> teamRepository,
            IRepository<TeamGameState> teamGameStateRepository,
            ITaskService taskService)
        {
            Check.Require(gameRepository != null, "gameRepository may not be null");
            Check.Require(teamRepository != null, "teamRepository may not be null");
            Check.Require(teamGameStateRepository != null, "teamGameStateRepository may not be null");
            Check.Require(taskService != null, "taskService may not be null");

            this.gameRepository = gameRepository;
            this.teamRepository = teamRepository;
            this.teamGameStateRepository = teamGameStateRepository;
            this.taskService = taskService;
        }

        private GameDemon GetGameDemon(int gameId)
        {
            if(gameDemon == null)
                gameDemon = new GameDemon(gameId, gameRepository, this);
            return gameDemon;
        }

        private static int Doers;
        private static DateTime LastUpdate = DateTime.Now;

        public void Do(int gameId)
        {
            // TODO: Сделать блокировку доступа к полю LastUpdate
            if ((DateTime.Now - LastUpdate).TotalSeconds < 2)
            {
                // Не позволяем производить пересчет состояния чаще 2 секунд
                return;
            }


            if (Doers > 0)
            {
                log4net.LogManager.GetLogger("LogToFile").Info(String.Format("Ахтунг!!! Doers = {0}", Doers));
            }

            try
            {
                Doers++;

                GetGameDemon(gameId).GameDemonCallback(null);
            }
            catch(Exception e)
            {
                log4net.LogManager.GetLogger("LogToFile").Info(String.Format("Ахтунг ошибка!!! Doers = {0}, {1}", Doers, e));
            }
            finally
            {
                Doers--;
                LastUpdate = DateTime.Now;
            }
        }

        #region Формирование отчета

        public DataTable GetReportDataTable(User user)
        {
            if (user.Role.IsAuthor)
            {
                DataTable dt = new DataTable("Report");

                dt.Columns.Add(new DataColumn("Команда", typeof(String)));
                foreach (Task task in user.Game.Tasks)
                {
                    dt.Columns.Add(new DataColumn(task.Name, typeof(String)));
                }

                foreach (Team team in user.Game.Teams)
                {
                    DataRow[] rows = new DataRow[] { dt.NewRow(), dt.NewRow(), dt.NewRow(), dt.NewRow(), dt.NewRow() };

                    rows[0][0] = team.Name;

                    if (team.TeamGameState != null)
                    {
                        foreach (TeamTaskState taskState in team.TeamGameState.AcceptedTasks)
                        {
                            int i = 0;
                            foreach (AcceptedTip tip in taskState.AcceptedTips)
                            {
                                rows[i++][taskState.Task.Name] = "Пол. в " + tip.AcceptTime.TimeOfDay;
                            }

                            rows[3][taskState.Task.Name] = "Пол. коды ";
                            foreach (AcceptedCode code in taskState.AcceptedCodes)
                            {
                                rows[3][taskState.Task.Name] = String.Format("{0} {1}", rows[3][taskState.Task.Name], code.Code.Name);
                            }

                            if (taskState.TaskFinishTime != null)
                            {
                                string state = String.Empty;
                                state = taskState.State == (int)TeamTaskStateFlag.Success ? "Вып" : state;
                                state = taskState.State == (int)TeamTaskStateFlag.Overtime ? "Невып" : state;
                                state = taskState.State == (int)TeamTaskStateFlag.Canceled ? "Слито" : state;
                                rows[4][taskState.Task.Name] = String.Format("{0} в {1}", state,((DateTime)taskState.TaskFinishTime).TimeOfDay);
                            }
                        }
                    }

                    dt.Rows.Add(rows[0]);
                    dt.Rows.Add(rows[1]);
                    dt.Rows.Add(rows[2]);
                    dt.Rows.Add(rows[3]);
                    dt.Rows.Add(rows[4]);
                }
                dt.AcceptChanges();
                return dt;
            }
            return null;
        }

        public Stream GetReport(User user)
        {
            DataTable dt = GetReportDataTable(user);
            if (dt != null)
            {
                MemoryStream ms = new MemoryStream();
                ZipOutputStream zipOutputStream = new ZipOutputStream(ms);
                ZipEntry zipEntry = new ZipEntry("report.csv");
                zipOutputStream.PutNextEntry(zipEntry);
                zipOutputStream.SetLevel(5);
                string st = CsvWriter.WriteToString(dt, true, false);
                zipOutputStream.Write(Encoding.Default.GetBytes(st), 0, st.Length);
                zipOutputStream.Finish();
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);

                return ms;
            }
            return null;
        }

        #endregion Формирование отчета

        #region Подсчет итогов

        public DataTable GetGameResults(int gameId)
        {
            DataTable dt = new DataTable("GameResults");
            DataColumn rankColumn = new DataColumn("rank", typeof(int));
            DataColumn teamColumn = new DataColumn("team", typeof(string));
            DataColumn tasksColumn = new DataColumn("tasks", typeof(int));
            DataColumn bonusColumn = new DataColumn("bonus", typeof(int));
            DataColumn timeColumn = new DataColumn("time", typeof(TimeSpan));
            dt.Columns.AddRange(new DataColumn[] { rankColumn, teamColumn, tasksColumn, bonusColumn, timeColumn });

            Game game = gameRepository.Get(gameId);

            // Выбираем команды закончившие игру
            foreach (Team team in game.Teams.Where(x => x.TeamGameState != null && x.TeamGameState.GameDoneTime != null))
            {
                // Количество успешно выполненных заданий
                int tasks = team.TeamGameState.AcceptedTasks.Count(t => t.State == (int) TeamTaskStateFlag.Success);

                // Количество бонусов
                int bonus = team.TeamGameState.AcceptedTasks.BonusCodesCount();

                // Время выполнения последнего успешно выполненного задания
                DateTime lastTaskTime = game.GameDate;
                var taskStates = team.TeamGameState.AcceptedTasks.Where(x => x.State == (int) TeamTaskStateFlag.Success);
                if (taskStates.Count() > 0)
                {
                    TeamTaskState tts = taskStates.Last();
                    lastTaskTime = (DateTime)tts.TaskFinishTime;
                }

                DataRow row = dt.NewRow();
                row[teamColumn] = team.Name;
                row[tasksColumn] = tasks;
                row[bonusColumn] = bonus;
                row[timeColumn] = lastTaskTime - game.GameDate;
                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion Подсчет итогов

        #region Управление игрой

        public void StartupGame(Game game)
        {
            Check.Require(game.GameState == (int)GameStates.Planned, String.Format(
                    "Невозможно перевести игру в предстартовый режим, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            Check.Require(!gameRepository.GetAll().Any(
                g =>
                g.GameState == (int) GameStates.Startup || g.GameState == (int) GameStates.Started ||
                g.GameState == (int) GameStates.Finished), 
                "Невозможно запустить игру, т.к. уже существует запущенная игра."
                );

            // Для каждой команды создаем игровое состояние
            foreach (Team team in game.Teams)
            {
                team.TeamGameState = new TeamGameState { Team = team, Game = game };
                teamGameStateRepository.SaveOrUpdate(team.TeamGameState);
                team.TeamGameStates.Add(team.TeamGameState);
            }
            
            // Переводим игру в предстартовый режим 
            game.GameState = (int)GameStates.Startup;
            
            gameRepository.SaveOrUpdate(game);
            teamGameStateRepository.DbContext.CommitChanges();
        }

        public void StartGame(Game game)
        {
            Check.Require(game.GameState == (int)GameStates.Startup, String.Format(
                    "Невозможно перевести игру в рабочий режим, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            // Переводим игру в рабочий режим 
            game.GameState = (int)GameStates.Started;
            gameRepository.SaveOrUpdate(game);

            // Запускаем демона
            GetGameDemon(game.Id).Start();
        }

        public void StopGame(Game game)
        {
            Check.Require(game.GameState == (int)GameStates.Started, String.Format(
                    "Невозможно остановить игру, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            // Останавливаем демона
            if (gameDemon != null)
            {
                gameDemon.Stop();
                gameDemon = null;
            }

            // Останавливаем игру
            game.GameState = (int)GameStates.Finished;
            gameRepository.SaveOrUpdate(game);

            // Для каждой команды устанавливаем время окончания игры
            foreach (Team team in game.Teams)
            {
                if (team.TeamGameState != null && team.TeamGameState.GameDoneTime == null)
                {
                    if (team.TeamGameState.ActiveTaskState != null)
                    {
                        taskService.CloseTaskForTeam(team.TeamGameState.ActiveTaskState, TeamTaskStateFlag.Overtime);
                    }

                    team.TeamGameState.GameDoneTime = DateTime.Now;
                    teamRepository.SaveOrUpdate(team);
                    teamGameStateRepository.SaveOrUpdate(team.TeamGameState);
                }
            }
            teamGameStateRepository.DbContext.CommitChanges();
        }

        public void CloseGame(Game game)
        {
            Check.Require(game.GameState == (int)GameStates.Finished || game.GameState == (int)GameStates.Planned, 
                String.Format(
                    "Невозможно закрыть игру, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            game.GameState = (int)GameStates.Cloused;
            gameRepository.SaveOrUpdate(game);

            // Для каждой команды сбрасываем игровое состояние
            foreach (Team team in game.Teams)
            {
                team.TeamGameState = null;
                team.Game = null;
                teamRepository.SaveOrUpdate(team);
            }

            teamGameStateRepository.DbContext.CommitChanges();
        }

        /// <summary>
        /// Сброс состояния игры.
        /// Переводит игру в начальное состояние, 
        /// удаляет состояния команд.
        /// </summary>
        /// <param name="game"></param>
        public void ResetGame(Game game)
        {
            Check.Require(
                game.GameState == (int)GameStates.Startup || 
                game.GameState == (int)GameStates.Finished ||
                game.GameState == (int)GameStates.Cloused ||
                game.GameState == (int)GameStates.Planned,
                String.Format(
                    "Невозможно сбросить состояние игры, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            game.GameState = (int)GameStates.Planned;
            gameRepository.SaveOrUpdate(game);

            // Для каждой команды сбрасываем игровое состояние
            foreach (Team team in game.Teams)
            {
                team.TeamGameState = null;
                teamRepository.SaveOrUpdate(team);
            }

            teamGameStateRepository.DbContext.CommitChanges();
        }

        #endregion Управление игрой

        #region Управление заданиями

        /// <summary>
        /// Обработка принятого кода от команды.
        /// </summary>
        /// <param name="codes">Принятый код.</param>
        /// <param name="teamGameState">Команда отправившая код.</param>
        /// <param name="user">Игрок отправившый код.</param>
        public void SubmitCode(string codes, TeamGameState teamGameState, User user)
        {
            taskService.SubmitCode(codes, teamGameState, user);
        }

        /// <summary>
        /// Помечает задание как успешно выполненное.
        /// </summary>
        /// <param name="teamTaskState"></param>
        public void CloseTaskForTeam(TeamTaskState teamTaskState, TeamTaskStateFlag flag)
        {
            taskService.CloseTaskForTeam(teamTaskState, flag);
        }

        /// <summary>
        /// Назначение нового задания команде.
        /// </summary>
        public void AssignNewTask(TeamGameState teamGameState, Task oldTask)
        {
            taskService.AssignNewTask(teamGameState, oldTask);
        }

        /// <summary>
        /// Отправить команде подсказку.
        /// </summary>
        /// <param name="teamTaskState"></param>
        /// <param name="tip"></param>
        public void AssignNewTaskTip(TeamTaskState teamTaskState, Tip tip)
        {
            taskService.AssignNewTaskTip(teamTaskState, tip);
        }

        /// <summary>
        /// "Ускориться".
        /// </summary>
        /// <param name="teamTaskState">Состояние команды затребовавшая ускорение.</param>
        public void AccelerateTask(TeamTaskState teamTaskState)
        {
            taskService.AccelerateTask(teamTaskState);
        }

        /// <summary>
        /// Возвращает варианты выбора подсказок, если это необходимо для задания с выбором подсказки.
        /// </summary>
        public IEnumerable<Tip> GetSuggestTips(TeamTaskState teamTaskState)
        {
            return taskService.GetSuggestTips(teamTaskState);
        }

        #endregion Управление заданиями
    }
}
