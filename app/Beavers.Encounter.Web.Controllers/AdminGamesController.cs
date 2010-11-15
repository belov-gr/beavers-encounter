using System.Web.Mvc;

using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;

using Beavers.Encounter.Common;
using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;

namespace Beavers.Encounter.Web.Controllers
{
    [AdministratorsOnly]
    public class AdminGamesController : BaseController
    {
        private readonly IRepository<Game> gameRepository;

        public AdminGamesController(IUserRepository userRepository,
            IRepository<Game> gameRepository) 
            : base(userRepository)
        {
            Check.Require(gameRepository != null, "gameRepository may not be null");

            this.gameRepository = gameRepository;
        }

        [Breadcrumb("Список игр", 3)]
        public ActionResult Index()
        {
            return View(gameRepository.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Game game)
        {
            if (ViewData.ModelState.IsValid && game.IsValid())
            {
                gameRepository.SaveOrUpdate(game);

                Message = "Игра успешно создана.";
                return this.RedirectToAction(c => c.Index());
            }

            return View(game);
        }

        [Breadcrumb("Игра \"{0}\"", 4)]
        public ActionResult Edit(int id)
        {
            var game = gameRepository.Get(id);
            this.SetBreadcrumbText(game.Name);
            return View(game);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Game game)
        {
            Game gameToUpdate = gameRepository.Get(game.Id);
            TransferFormValuesTo(gameToUpdate, game);

            if (ViewData.ModelState.IsValid && game.IsValid())
            {
                Message = "Игра успешно изменена.";
                return this.RedirectToAction(c => c.Index());
            }

            gameRepository.DbContext.RollbackTransaction();
            return View(game);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            string resultMessage = "Игра успешно удалена.";
            Game gameToDelete = gameRepository.Get(id);

            if (gameToDelete != null)
            {
                gameRepository.Delete(gameToDelete);

                try
                {
                    gameRepository.DbContext.CommitChanges();
                }
                catch
                {
                    resultMessage = "По ряду причин игра не может быть удалена. Возможно, она еще где-то нужна.";
                    gameRepository.DbContext.RollbackTransaction();
                }
            }
            else
            {
                resultMessage = "Удаляемая игра не найдена. Возможно, она уже удалена.";
            }

            Message = resultMessage;
            return this.RedirectToAction(c => c.Index());
        }

        private void TransferFormValuesTo(Game gameToUpdate, Game gameFromForm)
        {
            gameToUpdate.Name = gameFromForm.Name;
            gameToUpdate.GameDate = gameFromForm.GameDate;
            gameToUpdate.Description = gameFromForm.Description;
            gameToUpdate.TotalTime = gameFromForm.TotalTime;
        }
    }
}
