using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.ApplicationServices;
using Beavers.Encounter.Core;
using NUnit.Framework;
using Rhino.Mocks;
using SharpArch.Core.PersistenceSupport;
using Tests.TestHelpers;

namespace Tests.Beavers.Encounter.ApplicationServices
{
    [TestFixture]
    public class RecalcGameStateServiceTests
    {
        #region Setup

        private MockRepository mocks;
        private IGameService gameService;
        private IRepository<Game> repository;
        private RecalcGameStateService service;
        private Game game;
        private Task task1;
        private Tip task1Tip0;
        private Tip task1Tip1;
        private Tip task1Tip2;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            gameService = mocks.DynamicMock<IGameService>();
            repository = mocks.DynamicMock<IRepository<Game>>();
            service = new RecalcGameStateService(1, repository, gameService);

            game = new Game
            {
                GameState = GameStates.Started,
                GameDate = new DateTime(2010, 1, 1, 21, 0, 0),
                TotalTime = 540,
                TimePerTask = 90,
                TimePerTip = 30
            };
            
            task1 = new Task();
            task1Tip0 = new Tip { SuspendTime = 0, Task = task1 };
            task1Tip1 = new Tip { SuspendTime = 30, Task = task1 };
            task1Tip2 = new Tip { SuspendTime = 60, Task = task1 };
            task1.Tips.Add(task1Tip0);
            task1.Tips.Add(task1Tip1);
            task1.Tips.Add(task1Tip2);
            task1.Codes.Add(new Code { Name = "1", Task = task1 });

            game.Tasks.Add(task1);
        }

        #endregion Setup

        #region StopGame

        [Test]
        public void CanStopGameTest()
        {
            Expect.Call(repository.Get(1))
                .Return(game);

            Expect.Call(() => gameService.StopGame(game));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 2, 6, 0, 0));

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldNotStopGameTest()
        {
            Expect.Call(repository.Get(1))
                .Return(game).Repeat.Any();

            DoNotExpect.Call(() => gameService.StopGame(game));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 2, 5, 59, 59));
            service.RecalcGameState(new DateTime(2010, 1, 1, 21, 0, 0));
            service.RecalcGameState(new DateTime(2010, 1, 1, 20, 0, 0));

            mocks.VerifyAll();
        }

        #endregion StopGame

        #region AssignFirstTask

        [Test]
        public void CanAssignFirstTaskTest()
        {
            var tgs = new TeamGameState { Game = game };
            game.Teams.Add(new Team { TeamGameState = tgs });

            Expect.Call(repository.Get(1))
                .Return(game).Repeat.Any();

            Expect.Call(() => gameService.AssignNewTask(tgs, null));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 21, 0, 0));

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldNotAssignFirstTaskTest()
        {
            var team = new Team();
            var tgs = new TeamGameState { Game = game, Team = team };
            team.TeamGameState = tgs;
            game.Teams.Add(team);

            Expect.Call(repository.Get(1))
                .Return(game).Repeat.Any();

            DoNotExpect.Call(() => gameService.AssignNewTask(tgs, null));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 20, 59, 59));

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldNotAssignFirstTaskTest2()
        {
            var team = new Team();
            var tts = new TeamTaskState { Task = task1 };
            var tgs = new TeamGameState { Game = game, Team = team, ActiveTaskState = tts };
            team.TeamGameState = tgs;
            game.Teams.Add(team);

            Expect.Call(repository.Get(1))
                .Return(game).Repeat.Any();

            DoNotExpect.Call(() => gameService.AssignNewTask(tgs, null));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 22, 0, 0));

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldNotAssignFirstTaskTest3()
        {
            var team = new Team();
            var tts = new TeamTaskState { Task = task1 };
            var tgs = new TeamGameState { Game = game, Team = team };
            tgs.AcceptedTasks.Add(tts);
            team.TeamGameState = tgs;
            game.Teams.Add(team);

            Expect.Call(repository.Get(1))
                .Return(game).Repeat.Any();

            DoNotExpect.Call(() => gameService.AssignNewTask(tgs, null));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 22, 0, 0));

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldNotAssignFirstTaskTest4()
        {
            var team = new Team();
            var tts = new TeamTaskState { Task = task1 };
            var tgs = new TeamGameState { Game = game, Team = team, ActiveTaskState = tts };
            tgs.AcceptedTasks.Add(tts);
            team.TeamGameState = tgs;
            game.Teams.Add(team);

            Expect.Call(repository.Get(1))
                .Return(game).Repeat.Any();

            DoNotExpect.Call(() => gameService.AssignNewTask(tgs, null));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 22, 0, 0));

            mocks.VerifyAll();
        }

        #endregion AssignFirstTask

        #region AssignNewTaskTip

        [Test]
        public void CanAssignFirstTaskTipTest()
        {
            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(task1, new DateTime(2010, 1, 1, 21, 0, 0));

            Expect.Call(repository.Get(1))
                .Return(game);

            Expect.Call(() => gameService.AssignNewTaskTip(team.TeamGameState.ActiveTaskState, task1Tip0))
                .Do((Action<TeamTaskState, Tip>)((ts, tip) 
                    => team.TeamGameState.ActiveTaskState.AcceptedTips.Add(new AcceptedTip { 
                            Tip = tip, 
                            TeamTaskState = ts 
                        }))
                );

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 21, 0, 0));

            mocks.VerifyAll();

            Assert.AreEqual(1, team.TeamGameState.ActiveTaskState.AcceptedTips.Count());
        }

        [Test]
        public void CanAssignSecondTaskTipTest()
        {
            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(task1, new DateTime(2010, 1, 1, 21, 0, 0))
                .AssignTip(task1Tip0);

            Expect.Call(repository.Get(1))
                .Return(game);

            Expect.Call(() => gameService.AssignNewTaskTip(team.TeamGameState.ActiveTaskState, task1Tip0))
                .Do((Action<TeamTaskState, Tip>)((ts, tip)
                    => team.TeamGameState.ActiveTaskState.AcceptedTips.Add(new AcceptedTip {
                        Tip = tip,
                        TeamTaskState = ts
                    }))
                );

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 21, 30, 0));

            mocks.VerifyAll();

            Assert.AreEqual(2, team.TeamGameState.ActiveTaskState.AcceptedTips.Count());
        }

        [Test]
        public void SouldNotAssignTip4RussianRouletteTaskTest()
        {
            var russianRouletteTask = new Task { TaskType = TaskTypes.RussianRoulette };
            russianRouletteTask.Tips.Add(new Tip { SuspendTime = 0, Task = russianRouletteTask });
            russianRouletteTask.Tips.Add(new Tip { SuspendTime = 30, Task = russianRouletteTask });
            russianRouletteTask.Tips.Add(new Tip { SuspendTime = 60, Task = russianRouletteTask });

            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(russianRouletteTask, new DateTime(2010, 1, 1, 21, 0, 0));

            Expect.Call(repository.Get(1))
                .Return(game);

            DoNotExpect.Call(() => gameService.AssignNewTaskTip(null, null));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 21, 0, 0));

            mocks.VerifyAll();

            Assert.AreEqual(0, team.TeamGameState.ActiveTaskState.AcceptedTips.Count());
        }

        #endregion AssignNewTaskTip

        #region CheckExceededBadCodes

        [Test]
        public void CanCheckExceededBadCodesTest()
        {
            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(task1, new DateTime(2010, 1, 1, 21, 0, 0))
                .AssignTip(task1Tip0);

            Expect.Call(repository.Get(1))
                .Return(game);

            Expect.Call(() => gameService.CheckExceededBadCodes(team.TeamGameState));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 21, 30, 0));

            mocks.VerifyAll();
        }

        #endregion CheckExceededBadCodes

        #region CheckOvertime
        
        [Test]
        public void CheckSuccessCompleteTest()
        {
            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(task1, new DateTime(2010, 1, 1, 21, 0, 0))
                .AssignTip(task1Tip0)
                .AcceptCode(task1.Codes[0], new DateTime(2010, 1, 1, 21, 10, 0));

            Expect.Call(repository.Get(1)).Return(game);
            Expect.Call(() => gameService.CloseTaskForTeam(
                team.TeamGameState.ActiveTaskState, 
                TeamTaskStateFlag.Success));
            Expect.Call(() => gameService.AssignNewTask(team.TeamGameState, task1));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 22, 30, 0));

            mocks.VerifyAll();
        }

        [Test]
        public void CheckOvertimeTest()
        {
            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(task1, new DateTime(2010, 1, 1, 21, 0, 0))
                .AssignTip(task1Tip0);

            Expect.Call(repository.Get(1)).Return(game);
            Expect.Call(() => gameService.CloseTaskForTeam(
                team.TeamGameState.ActiveTaskState,
                TeamTaskStateFlag.Overtime));
            Expect.Call(() => gameService.AssignNewTask(team.TeamGameState, task1));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 22, 30, 0));

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldNotOvertimeTest()
        {
            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(task1, new DateTime(2010, 1, 1, 21, 0, 0))
                .AssignTip(task1Tip0);

            Expect.Call(repository.Get(1)).Return(game);
            DoNotExpect.Call(() => gameService.CloseTaskForTeam(
                team.TeamGameState.ActiveTaskState,
                TeamTaskStateFlag.Overtime));
            DoNotExpect.Call(() => gameService.AssignNewTask(team.TeamGameState, task1));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 22, 29, 59));

            mocks.VerifyAll();
        }

        [Test]
        public void CanOvertime4NeedForSpeedTaskTest()
        {
            var needForSpeedTask = new Task { TaskType = TaskTypes.NeedForSpeed };
            needForSpeedTask.Tips.Add(new Tip { SuspendTime = 60+10, Task = needForSpeedTask });
            needForSpeedTask.Codes.Add(new Code { Name = "1", Task = needForSpeedTask });

            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(needForSpeedTask, new DateTime(2010, 1, 1, 21, 0, 0));
            team.TeamGameState.ActiveTaskState.AccelerationTaskStartTime = new DateTime(2010, 1, 1, 22, 00, 0);

            Expect.Call(repository.Get(1)).Return(game);
            Expect.Call(() => gameService.CloseTaskForTeam(
                team.TeamGameState.ActiveTaskState,
                TeamTaskStateFlag.Overtime));
            Expect.Call(() => gameService.AssignNewTask(team.TeamGameState, task1));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 22, 20, 0));

            mocks.VerifyAll();
        }

        [Test]
        public void ShouldNotOvertime4NeedForSpeedTaskTest()
        {
            var needForSpeedTask = new Task { TaskType = TaskTypes.NeedForSpeed };
            needForSpeedTask.Tips.Add(new Tip { SuspendTime = 60 + 10, Task = needForSpeedTask });
            needForSpeedTask.Codes.Add(new Code { Name = "1", Task = needForSpeedTask });

            var team = new Team()
                .CreateTeamGameState(game)
                .AssignTask(needForSpeedTask, new DateTime(2010, 1, 1, 21, 0, 0));
            team.TeamGameState.ActiveTaskState.AccelerationTaskStartTime = new DateTime(2010, 1, 1, 22, 00, 0);

            Expect.Call(repository.Get(1)).Return(game);
            DoNotExpect.Call(() => gameService.CloseTaskForTeam(
                team.TeamGameState.ActiveTaskState,
                TeamTaskStateFlag.Overtime));
            DoNotExpect.Call(() => gameService.AssignNewTask(team.TeamGameState, task1));

            mocks.ReplayAll();

            service.RecalcGameState(new DateTime(2010, 1, 1, 22, 15, 0));

            mocks.VerifyAll();
        }

        #endregion CheckOvertime

        #region CanRecalc

        [Test]
        public void CanRecalcPlannedTest()
        {
            game.GameState = GameStates.Planned;

            Expect.Call(repository.Get(1))
                .Return(game);

            mocks.ReplayAll();

            service.RecalcGameState(DateTime.Now);

            mocks.VerifyAll();
        }

        [Test]
        public void CanRecalcStartupTest()
        {
            game.GameState = GameStates.Startup;

            Expect.Call(repository.Get(1))
                .Return(game);

            mocks.ReplayAll();

            service.RecalcGameState(DateTime.Now);

            mocks.VerifyAll();
        }

        [Test]
        public void CanRecalcFinishedTest()
        {
            game.GameState = GameStates.Finished;

            Expect.Call(repository.Get(1))
                .Return(game);

            mocks.ReplayAll();

            service.RecalcGameState(DateTime.Now);

            mocks.VerifyAll();
        }

        [Test]
        public void CanRecalcClousedTest()
        {
            game.GameState = GameStates.Cloused;

            Expect.Call(repository.Get(1))
                .Return(game);

            mocks.ReplayAll();

            service.RecalcGameState(DateTime.Now);

            mocks.VerifyAll();
        }

        #endregion CanRecalc
    }
}
