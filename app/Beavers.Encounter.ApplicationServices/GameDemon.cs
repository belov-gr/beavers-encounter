using System;
using System.Linq;
using Beavers.Encounter.Core;
using Beavers.Encounter.ServerInterface;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using log4net;
using System.Threading;
using System.Web;
using System.IO;

namespace Beavers.Encounter.ApplicationServices
{
    public sealed class GameDemon
    {
        private int gameId;
        private readonly IGameService gameService;
        private readonly IRepository<Game> gameRepository;
        private Timer timer;

        public GameDemon(int gameId, IRepository<Game> gameRepository, IGameService gameService)
        {
            Check.Require(gameRepository != null, "gameRepository may not be null");
            Check.Require(gameService != null, "gameService may not be null");

            this.gameId = gameId;
            this.gameRepository = gameRepository;
            this.gameService = gameService;
        }

        public void Start()
        {
            Check.Require(timer == null, "timer must be null");

            timer = new Timer(GameDemonCallback, null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 30));
        }

        public void Stop()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        private void SetDummyHttpContext()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("file", "http://local", "queryString"),
                new HttpResponse(new StringWriter()));
        }

        public void GameDemonCallback(object o)
        {
            SetDummyHttpContext();

            Game game = gameRepository.Get(gameId);
            if (game.GameState != GameStates.Started)
                return;

            CheckForGameFinish(game);
            foreach (Team team in game.Teams)
            {
                if (team.TeamGameState != null)
                {
                    CheckForFirstTask(team.TeamGameState);
                    CheckOvertime(team.TeamGameState);
                    CheckExceededBadCodes(team.TeamGameState);
                    CheckForNextTip(team.TeamGameState);
                }
            } 
        }

        private void CheckForGameFinish(Game game)
        {
            if ((DateTime.Now - game.GameDate).TotalMinutes > game.TotalTime && game.GameState == GameStates.Started)
            {
                gameService.StopGame(game);
            }
        }

        /// <summary>
        /// Проверка на необходимость выдачи первого задания.
        /// </summary>
        private void CheckForFirstTask(TeamGameState teamGameState)
        {
            if (teamGameState.Game.GameDate > DateTime.Now)
                return;

            if (teamGameState.AcceptedTasks.Count == 0 && teamGameState.ActiveTaskState == null)
            {
                gameService.AssignNewTask(teamGameState, null);    
            }
        }

        /// <summary>
        /// Проверка на необходимость выдачи подсказки.
        /// </summary>
        /// <param name="teamGameState"></param>
        private void CheckForNextTip(TeamGameState teamGameState)
        {
            if (teamGameState == null || teamGameState.ActiveTaskState == null)
                return;

            // Для заданий с выбором подсказок ничерта не делаем
            if (teamGameState.ActiveTaskState.Task.TaskType == TaskTypes.RussianRoulette)
                return;

            // время выполнения задания
            TimeSpan taskTime = DateTime.Now - teamGameState.ActiveTaskState.TaskStartTime;

            foreach (Tip tip in teamGameState.ActiveTaskState.Task.Tips)
            {
                if (taskTime.TotalMinutes >= tip.SuspendTime)
                {
                    // если подсказка еще не получена
                    if (!teamGameState.ActiveTaskState.AcceptedTips.Any(t => t.Tip == tip))
                    {
                        // отправляем команде подсказку
                        gameService.AssignNewTaskTip(teamGameState.ActiveTaskState, tip);
                    }
                }
            } 
        }

        /// <summary>
        /// Проверка на превышение количества левых кодов. При превышении задание закрывается сразу перед первой подсказкой.
        /// </summary>
        /// <param name="teamGameState"></param>
        public void CheckExceededBadCodes(TeamGameState teamGameState)
        {
            gameService.CheckExceededBadCodes(teamGameState);
        }

        /// <summary>
        /// Проверка на перебор времени по заданию.
        /// </summary>
        /// <param name="teamGameState"></param>
        private void CheckOvertime(TeamGameState teamGameState)
        {
            if (teamGameState == null || teamGameState.ActiveTaskState == null)
                return;

            // время выполнения задания
            TimeSpan taskTime = DateTime.Now - teamGameState.ActiveTaskState.TaskStartTime;
            int timePerTask = teamGameState.Game.TimePerTask;

            // Если задание с "ускорением" и "ускорение" произошло
            if (teamGameState.ActiveTaskState.Task.TaskType == TaskTypes.NeedForSpeed &&
                teamGameState.ActiveTaskState.AccelerationTaskStartTime != null)
            {
                taskTime = DateTime.Now - (DateTime)teamGameState.ActiveTaskState.AccelerationTaskStartTime;
                timePerTask = teamGameState.Game.TimePerTask - teamGameState.ActiveTaskState.Task.Tips.Last(tip => tip.SuspendTime > 0).SuspendTime;
            }

            if (taskTime.TotalMinutes > timePerTask)
            {
                // Если все основные коды приняты, то задание считаем выполненым успешно
                TeamTaskStateFlag closeFlag =
                    teamGameState.ActiveTaskState.AcceptedCodes.Count(x => !x.Code.IsBonus) == teamGameState.ActiveTaskState.Task.Codes.Count(x => !x.IsBonus)
                    ? TeamTaskStateFlag.Success
                    : TeamTaskStateFlag.Overtime;
                
                Task oldTask = teamGameState.ActiveTaskState.Task;
                gameService.CloseTaskForTeam(teamGameState.ActiveTaskState, closeFlag);
                gameService.AssignNewTask(teamGameState, oldTask);
            }
        }
    }
}
