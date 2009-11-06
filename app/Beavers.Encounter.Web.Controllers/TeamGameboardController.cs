using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Beavers.Encounter.ApplicationServices;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.Web.Controllers.Filters;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;

namespace Beavers.Encounter.Web.Controllers
{
    [TeamGameboard]
    [Authorize]
    [LockIfGameStart]
    [HandleError]
    public class TeamGameboardController : BaseController
    {
        private readonly IGameService gameService;
        private readonly IRepository<Team> teamRepository;

        public TeamGameboardController(IRepository<Team> teamRepository, IUserRepository userRepository, IGameService gameService)
            : base(userRepository)
        {
            Check.Require(teamRepository != null, "teamRepository may not be null");
            Check.Require(gameService != null, "gameService may not be null");

            this.teamRepository = teamRepository;
            this.gameService = gameService;
        }

        //
        // GET: /TeamGameboard/Show
        [Transaction]
        public ActionResult Show()
        {
            if (User.Role.IsAuthor)
                return this.RedirectToAction<GamesController>(c => c.CurrentState(User.Game.Id));

            Team team = teamRepository.Get(User.Team.Id);

            TeamGameboardViewModel viewModel = TeamGameboardViewModel.CreateTeamGameboardViewModel();

            viewModel.ErrorMessage = Message;

            if (team.Game == null)
            {
                viewModel.Message = "Для Вас нет активной игры.";
            }
            else if (team.Game.GameState == GameStates.Finished)
            {
                viewModel.Message = "Игра закончена.";
                return RedirectToAction("Results");
            }
            else if (team.Game.GameState == GameStates.Planned)
            {
                viewModel.Message = String.Format("Игра запланирована на {0}",
                                                  team.Game.GameDate.ToShortDateString());
            }
            else if (team.Game.GameState == GameStates.Startup ||
                    (team.Game.GameState == GameStates.Started && team.Game.GameDate > DateTime.Now))
            {
                viewModel.Message = String.Format("Игра начнется в {0}",
                                                  team.TeamGameState.Game.GameDate.TimeOfDay);
            }
            else if (team.TeamGameState.GameDoneTime != null)
            {
                viewModel.Message =
                    "Игра закончена. Для вас пока больше нет заданий, возможно Вы выполнили все задания:)";
                return RedirectToAction("Results");
            }
            else
            {
                viewModel.TaskNo = team.TeamGameState.AcceptedTasks.Count;
                viewModel.TeamGameState = team.TeamGameState;
                viewModel.ActiveTaskState = team.TeamGameState.ActiveTaskState;

                if (viewModel.ActiveTaskState != null &&
                    viewModel.ActiveTaskState.Task.TaskType == TaskTypes.RussianRoulette)
                {
                    viewModel.SuggestTips = gameService.GetSuggestTips(viewModel.ActiveTaskState);

                    if (viewModel.SuggestTips != null)
                    {
                        viewModel.SuggestMessage = viewModel.SuggestTips.Count() == 3
                            ? "У вас есть две подсказки из трёх, какую вы выберите:"
                            : "У вас осталась последняя подсказка, какую вы выберите:";
                    }
                }
            }

            viewModel.TeamName = team.Name;

            return View(viewModel);
        }

        //
        // POST: /TeamGameboard/SubmitCodes
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitCodes(string codes)
        {
            Team team = teamRepository.Get(User.Team.Id);
            try
            {
                gameService.SubmitCode(codes, team.TeamGameState, User);
            }
            catch (MaxCodesCountException e)
            {
                Message = e.Message;
            }
            return this.RedirectToAction(c => c.Show());
        }

        //
        // POST: /TeamGameboard/NextTask
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NextTask(int activeTaskStateId)
        {
            Team team = teamRepository.Get(User.Team.Id);

            if (team.TeamGameState.ActiveTaskState.Id == activeTaskStateId)
            { // TODO: Перенести в gameService
                Task oldTask = team.TeamGameState.ActiveTaskState.Task;
                if (team.TeamGameState.ActiveTaskState.AcceptedCodes.Count(x => x.Code.IsBonus == 0) == team.TeamGameState.ActiveTaskState.Task.Codes.Count(x => x.IsBonus == 0))
                {
                    gameService.CloseTaskForTeam(team.TeamGameState.ActiveTaskState, TeamTaskStateFlag.Success);
                    gameService.AssignNewTask(team.TeamGameState, oldTask);
                }
            }

            return this.RedirectToAction(c => c.Show());
        }

        //
        // POST: /TeamGameboard/NextTask
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SkipTask()
        {
            Team team = teamRepository.Get(User.Team.Id);

            { // TODO: Перенести в gameService
                Task oldTask = team.TeamGameState.ActiveTaskState.Task;
                gameService.CloseTaskForTeam(team.TeamGameState.ActiveTaskState, TeamTaskStateFlag.Canceled);
                gameService.AssignNewTask(team.TeamGameState, oldTask);
            }

            return this.RedirectToAction(c => c.Show());
        }

        //
        // POST: /TeamGameboard/AccelerateTask
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AccelerateTask(int activeTaskStateId)
        {
            Team team = teamRepository.Get(User.Team.Id);
            if (team.TeamGameState != null &&
                team.TeamGameState.ActiveTaskState != null &&
                team.TeamGameState.ActiveTaskState.Id == activeTaskStateId &&
                team.TeamGameState.ActiveTaskState.Task.TaskType == TaskTypes.NeedForSpeed)
            {
                gameService.AccelerateTask(team.TeamGameState.ActiveTaskState);
            }

            return this.RedirectToAction(c => c.Show());
        }

        //
        // POST: /TeamGameboard/SelectTip
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SelectTip(int activeTaskStateId, int tipId)
        {
            Team team = teamRepository.Get(User.Team.Id);
            if (team.TeamGameState != null &&
                team.TeamGameState.ActiveTaskState != null &&
                team.TeamGameState.ActiveTaskState.Id == activeTaskStateId &&
                team.TeamGameState.ActiveTaskState.Task.TaskType == TaskTypes.RussianRoulette)
            {
                gameService.AssignNewTaskTip(
                    team.TeamGameState.ActiveTaskState,
                    team.TeamGameState.ActiveTaskState.Task.Tips.Single(tip => tip.Id == tipId));
            }

            return this.RedirectToAction(c => c.Show());
        }

        //
        // GET: /TeamGameboard/Results
        [Transaction]
        public ActionResult Results()
        {
            if (User.Role.IsAuthor)
                return this.RedirectToAction<GamesController>(c => c.CurrentState(User.Game.Id));

            TeamGameboardViewModel model = TeamGameboardViewModel.CreateTeamGameboardViewModel();
            model.GameResults = gameService.GetGameResults(User.Team.Game.Id).Select("", "tasks desc, bonus desc, time asc");
            
            Team team = teamRepository.Get(User.Team.Id);
            model.TeamGameState = team.TeamGameState;

            return View(model);
        }

        public class TeamGameboardViewModel
        {
            private TeamGameboardViewModel() { }

            public static TeamGameboardViewModel CreateTeamGameboardViewModel()
            {
                return new TeamGameboardViewModel();
            }

            public String Message { get; internal set; }
            public String ErrorMessage { get; internal set; }
            public String TeamName { get; internal set; }
            public int TaskNo { get; internal set; }
            public TeamGameState TeamGameState { get; internal set; }
            public TeamTaskState ActiveTaskState { get; internal set; }
            public IEnumerable<Tip> SuggestTips { get; internal set; }
            public String SuggestMessage { get; internal set; }
            public DataRow[] GameResults { get; set; }
        }
    }
}
