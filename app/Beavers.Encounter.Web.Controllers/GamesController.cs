using System.Data;
using System.Web.Mvc;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Web.Controllers.Filters;
using SharpArch.Core.PersistenceSupport;
using System.Collections.Generic;
using SharpArch.Web.NHibernate;
using SharpArch.Core;
using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.ApplicationServices;

namespace Beavers.Encounter.Web.Controllers
{
    [GameState]
    [LockIfGameStart]
    [HandleError]
    public class GamesController : BaseController
    {
        private readonly IRepository<Game> gameRepository;
        private readonly IGameService gameService;

        public GamesController(IRepository<Game> gameRepository, IUserRepository userRepository, IGameService gameService)
            : base(userRepository)
        {
            Check.Require(gameRepository != null, "gameRepository may not be null");
            Check.Require(gameService != null, "gameService may not be null");

            this.gameRepository = gameRepository;
            this.gameService = gameService;
        }

        [Transaction]
        public ActionResult Index() {
            IList<Game> games = gameRepository.GetAll();

			return View(games);
        }

        [Transaction]
        public ActionResult Show(int id) {
            Game game = gameRepository.Get(id);
            return View(game);
        }

        [GameOwner]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult StartupGame(int id)
        {
            gameService.StartupGame(gameRepository.Get(id));
            return this.RedirectToAction(c => c.CurrentState(id));
        }

        [GameOwner]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult StartGame(int id)
        {
            gameService.StartGame(gameRepository.Get(id));
            return this.RedirectToAction(c => c.CurrentState(id));
        }

        [GameOwner]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult StopGame(int id)
        {
            gameService.StopGame(gameRepository.Get(id));
            return this.RedirectToAction(c => c.Edit(id));
        }

        [GameOwner]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CloseGame(int id)
        {
            gameService.CloseGame(gameRepository.Get(id));
            return this.RedirectToAction(c => c.Edit(id));
        }

        [GameOwner]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ResetGame(int id)
        {
            gameService.ResetGame(gameRepository.Get(id));
            return this.RedirectToAction(c => c.Edit(id));
        }

        [GameOwner]
        public ActionResult CurrentState(int id)
        {
            GameStateViewModel viewModel = GameStateViewModel.CreateGameStateViewModel();

            viewModel.Game = gameRepository.Get(id);

            if (viewModel.Game.GameState == GameStates.Finished ||
                viewModel.Game.GameState == GameStates.Cloused)
            {
                viewModel.GameResults = gameService.GetGameResults(
                    viewModel.Game.Id).Select("", "tasks desc, bonus desc, time asc");
            }

            return View(viewModel);
        }

        public ActionResult Create()
        {
            GameFormViewModel viewModel = GameFormViewModel.CreateGameFormViewModel();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Game game) {
            if (ViewData.ModelState.IsValid && game.IsValid()) {
                gameRepository.SaveOrUpdate(game);

                // Делаем текущего пользователя автором созданной игры
                User.Game = game;
                UserRepository.SaveOrUpdate(User);

                Message = "Игра успешно создана.";
                return this.RedirectToAction(c => c.Index());
            }

            GameFormViewModel viewModel = GameFormViewModel.CreateGameFormViewModel();
            viewModel.Game = game;
            return View(viewModel);
        }

        [GameOwner]
        [Transaction]
        public ActionResult Edit(int id) {
            GameFormViewModel viewModel = GameFormViewModel.CreateGameFormViewModel();
            viewModel.Game = gameRepository.Get(id);
            return View(viewModel);
        }

        [GameOwner]
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Game game) {
            Game gameToUpdate = gameRepository.Get(game.Id);
            TransferFormValuesTo(gameToUpdate, game);

            if (ViewData.ModelState.IsValid && game.IsValid()) {
                Message = "Игра успешно изменена.";
                return this.RedirectToAction(c => c.Index());
            }
            
            gameRepository.DbContext.RollbackTransaction();

			GameFormViewModel viewModel = GameFormViewModel.CreateGameFormViewModel();
			viewModel.Game = game;
			return View(viewModel);
        }

        private void TransferFormValuesTo(Game gameToUpdate, Game gameFromForm) {
			gameToUpdate.Name = gameFromForm.Name;
			gameToUpdate.GameDate = gameFromForm.GameDate;
			gameToUpdate.Description = gameFromForm.Description;
			gameToUpdate.TotalTime = gameFromForm.TotalTime;
			gameToUpdate.TimePerTask = gameFromForm.TimePerTask;
			gameToUpdate.TimePerTip = gameFromForm.TimePerTip;
            gameToUpdate.PrefixMainCode = gameFromForm.PrefixMainCode;
            gameToUpdate.PrefixBonusCode = gameFromForm.PrefixBonusCode;
        }

        [GameOwner]
        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            string resultMessage = "Игра успешно удалена.";
            Game gameToDelete = gameRepository.Get(id);

            if (gameToDelete != null) {
                User.Game = null;
                gameRepository.Delete(gameToDelete);

                try {
                    gameRepository.DbContext.CommitChanges();
                }
                catch {
                    resultMessage = "По ряду причин игра не может быть удалена. Возможно, она еще где-то нужна.";
                    gameRepository.DbContext.RollbackTransaction();
                }
            }
            else {
                resultMessage = "Удаляемая игра не найдена. Возможно, она уже удалена.";
            }

            Message = resultMessage;
            return this.RedirectToAction(c => c.Index());
        }

		/// <summary>
		/// Holds data to be passed to the Game form for creates and edits
		/// </summary>
        public class GameFormViewModel
        {
            private GameFormViewModel() { }

			/// <summary>
			/// Creation method for creating the view model. Services may be passed to the creation 
			/// method to instantiate items such as lists for drop down boxes.
			/// </summary>
            public static GameFormViewModel CreateGameFormViewModel() {
                GameFormViewModel viewModel = new GameFormViewModel();
                
                return viewModel;
            }

            public Game Game { get; internal set; }
        }

        public class GameStateViewModel
        {
            private GameStateViewModel() { }

            /// <summary>
            /// Creation method for creating the view model. Services may be passed to the creation 
            /// method to instantiate items such as lists for drop down boxes.
            /// </summary>
            public static GameStateViewModel CreateGameStateViewModel()
            {
                GameStateViewModel viewModel = new GameStateViewModel();

                return viewModel;
            }

            public Game Game { get; internal set; }
            public DataRow[] GameResults { get; internal set; }
        }
    }
}
