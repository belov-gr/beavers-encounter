using System;
using Beavers.Encounter.Core;

namespace Tests.TestHelpers
{
    public static class CoreExtensions
    {
        public static Team CreateTeamGameState(this Team team, Game game)
        {
            var tgs = new TeamGameState
            {
                Game = game,
                Team = team
            };
            team.TeamGameState = tgs;
            team.Game = game;
            game.Teams.Add(team);
            return team;
        }

        public static Team AssignTask(this Team team, Task task, DateTime taskStartTime)
        {
            var tts = new TeamTaskState
            {
                Task = task,
                TaskStartTime = taskStartTime,
                TeamGameState = team.TeamGameState
            };
            team.TeamGameState.ActiveTaskState = tts;
            return team;
        }

        public static Team AssignTip(this Team team, Tip tip)
        {
            team.TeamGameState.ActiveTaskState.AcceptedTips.Add(new AcceptedTip
            {
                Tip = tip,
                TeamTaskState = team.TeamGameState.ActiveTaskState
            });
            return team;
        }

        public static Team AcceptCode(this Team team, Code code, DateTime acceptTime)
        {
            team.TeamGameState.ActiveTaskState.AcceptedCodes.Add(new AcceptedCode
            {
                AcceptTime = acceptTime,
                Code = code,
                TeamTaskState = team.TeamGameState.ActiveTaskState
            });
            return team;
        }
    }
}
