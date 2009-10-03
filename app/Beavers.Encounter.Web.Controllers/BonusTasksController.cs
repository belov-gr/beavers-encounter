using System.Web.Mvc;
using Beavers.Encounter.Web.Controllers.Binders;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.DomainModel;
using System.Collections.Generic;
using System;
using SharpArch.Web.NHibernate;
using NHibernate.Validator.Engine;
using System.Text;
using SharpArch.Web.CommonValidator;
using SharpArch.Core;

using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;


namespace Beavers.Encounter.Web.Controllers
{
    [AuthorsOnly]
    [HandleError]
    public class BonusTasksController : BaseController
    {
        public BonusTasksController(IRepository<BonusTask> bonusTaskRepository, IUserRepository userRepository)
            : base(userRepository)
        {
            Check.Require(bonusTaskRepository != null, "bonusTaskRepository may not be null");

            this.bonusTaskRepository = bonusTaskRepository;
        }

        [Transaction]
        public ActionResult Show(int id) {
            BonusTask bonusTask = bonusTaskRepository.Get(id);
            return View(bonusTask);
        }

        public ActionResult Create() {
            BonusTaskFormViewModel viewModel = BonusTaskFormViewModel.CreateBonusTaskFormViewModel();

            viewModel.StartTime = User.Game.GameDate.AddMinutes(60);
            viewModel.FinishTime = User.Game.GameDate.AddMinutes(User.Game.TotalTime) ;

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([BonusTaskBinder(Fetch = false)] BonusTask bonusTask) 
        {
            if (ViewData.ModelState.IsValid && bonusTask.IsValid()) 
            {
                bonusTask.Game = User.Game;
                bonusTaskRepository.SaveOrUpdate(bonusTask);

                TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = 
					"The bonusTask was successfully created.";
                return this.RedirectToAction<GamesController>(c => c.Edit(bonusTask.Game.Id));
            }

            BonusTaskFormViewModel viewModel = BonusTaskFormViewModel.CreateBonusTaskFormViewModel();
            viewModel.BonusTask = bonusTask;
            return View(viewModel);
        }

        [Transaction]
        public ActionResult Edit(int id) {
            BonusTaskFormViewModel viewModel = BonusTaskFormViewModel.CreateBonusTaskFormViewModel();
            viewModel.BonusTask = bonusTaskRepository.Get(id);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([BonusTaskBinder(Fetch = true)] BonusTask bonusTask)
        {
            BonusTask bonusTaskToUpdate = bonusTaskRepository.Get(bonusTask.Id);
            TransferFormValuesTo(bonusTaskToUpdate, bonusTask);

            if (ViewData.ModelState.IsValid && bonusTask.IsValid()) {
                Message = "The bonusTask was successfully updated.";
                return this.RedirectToAction<GamesController>(c => c.Edit(bonusTaskToUpdate.Game.Id));
            }
            else {
                bonusTaskRepository.DbContext.RollbackTransaction();

				BonusTaskFormViewModel viewModel = BonusTaskFormViewModel.CreateBonusTaskFormViewModel();
				viewModel.BonusTask = bonusTask;
				return View(viewModel);
            }
        }

        private void TransferFormValuesTo(BonusTask bonusTaskToUpdate, BonusTask bonusTaskFromForm) {
			bonusTaskToUpdate.Name = bonusTaskFromForm.Name;
			bonusTaskToUpdate.TaskText = bonusTaskFromForm.TaskText;
			bonusTaskToUpdate.StartTime = bonusTaskFromForm.StartTime;
			bonusTaskToUpdate.FinishTime = bonusTaskFromForm.FinishTime;
            bonusTaskToUpdate.IsIndividual = bonusTaskFromForm.IsIndividual;
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            string resultMessage = "The bonusTask was successfully deleted.";
            BonusTask bonusTaskToDelete = bonusTaskRepository.Get(id);
            int gameId = bonusTaskToDelete.Game.Id;

            if (bonusTaskToDelete != null) 
            {
                bonusTaskRepository.Delete(bonusTaskToDelete);

                try 
                {
                    bonusTaskRepository.DbContext.CommitChanges();
                }
                catch 
                {
                    resultMessage = "A problem was encountered preventing the bonusTask from being deleted. " +
						"Another item likely depends on this bonusTask.";
                    bonusTaskRepository.DbContext.RollbackTransaction();
                }
            }
            else 
            {
                resultMessage = "The bonusTask could not be found for deletion. It may already have been deleted.";
            }

            Message = resultMessage;
            return this.RedirectToAction<GamesController>(c => c.Edit(gameId));
        }

		/// <summary>
		/// Holds data to be passed to the BonusTask form for creates and edits
		/// </summary>
        public class BonusTaskFormViewModel
        {
            private BonusTaskFormViewModel() { }

			/// <summary>
			/// Creation method for creating the view model. Services may be passed to the creation 
			/// method to instantiate items such as lists for drop down boxes.
			/// </summary>
            public static BonusTaskFormViewModel CreateBonusTaskFormViewModel() {
                BonusTaskFormViewModel viewModel = new BonusTaskFormViewModel();

                return viewModel;
            }

            public BonusTask BonusTask { get; internal set; }
            public DateTime StartTime { get; internal set; }
            public DateTime FinishTime { get; internal set; }
        }

        private readonly IRepository<BonusTask> bonusTaskRepository;
    }
}
