using System;
using System.Linq;
using System.Threading;
using Beavers.Encounter.Core;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.ApplicationServices
{
    public sealed class GameDemon
    {
        private int gameId;
        private readonly IGameService gameService;
        private readonly IRepository<Game> gameRepository;

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
            //demonTimer = new Timer(GameDemonCallback, null, 0, 5000000);
        }

        public void Stop()
        {
        }

        public void GameDemonCallback(object o)
        {
            Game game = gameRepository.Get(gameId);
            if (game.GameState != (int)GameStates.Started)
                return;
            
            foreach (Team team in game.Teams)
            {
                if (team.TeamGameState != null)
                {
                    CheckForGameFinish(game);
                    CheckForFirstTask(team.TeamGameState);
                    CheckOvertime(team.TeamGameState);
                    CheckForNextTip(team.TeamGameState);
                }
            } 
        }

        private void CheckForGameFinish(Game game)
        {
            if ((DateTime.Now - game.GameDate).TotalMinutes > game.TotalTime && game.GameState == (int)GameStates.Started)
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
            if (teamGameState.ActiveTaskState.Task.TaskType == (int)TaskTypes.NeedForSpeed &&
                teamGameState.ActiveTaskState.AccelerationTaskStartTime != null)
            {
                taskTime = DateTime.Now - (DateTime)teamGameState.ActiveTaskState.AccelerationTaskStartTime;
                timePerTask = teamGameState.Game.TimePerTask - teamGameState.ActiveTaskState.Task.Tips.Last(tip => tip.SuspendTime > 0).SuspendTime;
            }

            if (taskTime.TotalMinutes > timePerTask)
            {
                // Если все основные коды приняты, то задание считаем выполненым успешно
                TeamTaskStateFlag closeFlag =
                    teamGameState.ActiveTaskState.AcceptedCodes.Count(x => x.Code.IsBonus == 0) == teamGameState.ActiveTaskState.Task.Codes.Count(x => x.IsBonus == 0)
                    ? TeamTaskStateFlag.Success
                    : TeamTaskStateFlag.Overtime;
                
                Task oldTask = teamGameState.ActiveTaskState.Task;
                gameService.CloseTaskForTeam(teamGameState.ActiveTaskState, closeFlag);
                gameService.AssignNewTask(teamGameState, oldTask);
            }
        }
    }
}
