using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;

namespace Beavers.Encounter.ApplicationServices
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> gameRepository;
        private readonly IRepository<Team> teamRepository;
        private readonly IRepository<TeamGameState> teamGameStateRepository;
        private readonly ITaskService taskService;

        private IGameDemon gameDemon;

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

        public GameService(
            IRepository<Game> gameRepository,
            IRepository<Team> teamRepository,
            IRepository<TeamGameState> teamGameStateRepository,
            ITaskService taskService,
            IGameDemon gameDemon)
            : this(gameRepository, teamRepository, teamGameStateRepository, taskService)
        {
            Check.Require(gameDemon != null, "gameDemon may not be null");

            this.gameDemon = gameDemon;
        }

        private IGameDemon GetGameDemon(int gameId)
        {
            if (gameDemon == null)
                gameDemon = new GameDemon(new RecalcGameStateService(gameId, gameRepository, this));
            return gameDemon;
        }

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
                int tasks = team.TeamGameState.AcceptedTasks.Count(t => t.State == (int)TeamTaskStateFlag.Success);

                // Количество бонусов
                int bonus = team.TeamGameState.AcceptedTasks.BonusCodesCount();

                // Время выполнения последнего успешно выполненного задания
                DateTime lastTaskTime = game.GameDate;
                var taskStates = team.TeamGameState.AcceptedTasks.Where(x => x.State == (int)TeamTaskStateFlag.Success);
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
            Check.Require(game.GameState == GameStates.Planned, String.Format(
                    "Невозможно перевести игру в предстартовый режим, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            Check.Require(!gameRepository.GetAll().Any(
                g =>
                g.GameState == GameStates.Startup || g.GameState == GameStates.Started ||
                g.GameState == GameStates.Finished),
                "Невозможно запустить игру, т.к. уже существует запущенная игра."
                );

            // Для каждой команды, имеющей игроков, создаем игровое состояние
            foreach (Team team in game.Teams.Where(t => t.Users.Count > 0))
            {
                team.TeamGameState = new TeamGameState { Team = team, Game = game };
                teamGameStateRepository.SaveOrUpdate(team.TeamGameState);
                team.TeamGameStates.Add(team.TeamGameState);
            }

            // Переводим игру в предстартовый режим 
            game.GameState = GameStates.Startup;

            gameRepository.SaveOrUpdate(game);
            teamGameStateRepository.DbContext.CommitChanges();
        }

        public void StartGame(Game game)
        {
            Check.Require(game.GameState == GameStates.Startup, String.Format(
                    "Невозможно перевести игру в рабочий режим, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            // Переводим игру в рабочий режим 
            game.GameState = GameStates.Started;
            gameRepository.SaveOrUpdate(game);

            // Запускаем демона
            GetGameDemon(game.Id).Start();
        }

        public void StopGame(Game game)
        {
            Check.Require(game.GameState == GameStates.Started, String.Format(
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
            game.GameState = GameStates.Finished;
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
            Check.Require(game.GameState == GameStates.Finished,
                String.Format(
                    "Невозможно закрыть игру, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            game.GameState = GameStates.Cloused;
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
                game.GameState == GameStates.Startup ||
                game.GameState == GameStates.Finished ||
                game.GameState == GameStates.Cloused ||
                game.GameState == GameStates.Planned,
                String.Format(
                    "Невозможно сбросить состояние игры, когда она находится в режиме {0}.",
                    Enum.GetName(typeof(GameStates), game.GameState))
                );

            game.GameState = GameStates.Planned;
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

        #region Управление заданиями (Делегирование вызовов taskService)

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
        /// Проверка на превышение количества левых кодов. При превышении задание закрывается сразу перед первой подсказкой.
        /// </summary>
        public void CheckExceededBadCodes(TeamGameState teamGameState)
        {
            taskService.CheckExceededBadCodes(teamGameState);
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
