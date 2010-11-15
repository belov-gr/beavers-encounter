using System.Web.Mvc;
using Beavers.Encounter.Common;
using Beavers.Encounter.Core;
using Beavers.Encounter.Web.Controllers.Binders;
using Beavers.Encounter.Web.Controllers.Filters;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using SharpArch.Core;
using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.Core.DataInterfaces;

namespace Beavers.Encounter.Web.Controllers
{
    [TipOwner]
    [HandleError]
    public class TipsController : BaseController
    {
        public TipsController(IRepository<Tip> tipRepository, IRepository<Task> taskRepository, IUserRepository userRepository)
            : base(userRepository)
        {
            Check.Require(tipRepository != null, "tipRepository may not be null");
            Check.Require(taskRepository != null, "taskRepository may not be null");

            this.tipRepository = tipRepository;
            this.taskRepository = taskRepository;
        }

        [Breadcrumb("Новая подсказка", 5)]
        public ActionResult Create(int taskId)
        {
            TipFormViewModel viewModel = TipFormViewModel.CreateTipFormViewModel();
            viewModel.Tip = new Tip { Name = "Новая подсказка", SuspendTime = 30};
            viewModel.TaskId = taskId;

            Task task = taskRepository.Get(taskId);
            viewModel.Tip.SuspendTime = task.Tips.Count * task.Game.TimePerTip;
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([TipBinder(Fetch = false)] Tip tip) 
        {
            if (ViewData.ModelState.IsValid && tip.IsValid()) 
            {
                taskRepository.Get(tip.Task.Id).Tips.Add(tip);
                tipRepository.SaveOrUpdate(tip);
                taskRepository.DbContext.CommitChanges();

                Message = "Подсказка успешно создана.";
                return this.RedirectToAction<TasksController>(c => c.Edit(tip.Task.Id));
            }

            TipFormViewModel viewModel = TipFormViewModel.CreateTipFormViewModel();
            viewModel.Tip = tip;
            return View(viewModel);
        }

        [Breadcrumb("Подсказка", 5)]
        public ActionResult Edit(int id)
        {
            TipFormViewModel viewModel = TipFormViewModel.CreateTipFormViewModel();
            viewModel.Tip = tipRepository.Get(id);
            viewModel.TaskId = viewModel.Tip.Task.Id;
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([TipBinder(Fetch = true)]Tip tip)
        {
            Tip tipToUpdate = tipRepository.Get(tip.Id);
            TransferFormValuesTo(tipToUpdate, tip);

            if (ViewData.ModelState.IsValid && tip.IsValid()) {
                Message = "Подсказка успешно изменена.";
                return this.RedirectToAction<TasksController>(c => c.Edit(tip.Task.Id));
            }
            else {
                tipRepository.DbContext.RollbackTransaction();

				TipFormViewModel viewModel = TipFormViewModel.CreateTipFormViewModel();
				viewModel.Tip = tip;
				return View(viewModel);
            }
        }

        private void TransferFormValuesTo(Tip tipToUpdate, Tip tipFromForm) {
			tipToUpdate.Name = tipFromForm.Name;
			tipToUpdate.SuspendTime = tipFromForm.SuspendTime;
			tipToUpdate.Task = tipFromForm.Task;
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            string resultMessage = "Подсказка успешно удалена.";
            Tip tipToDelete = tipRepository.Get(id);
            int taskId = tipToDelete.Task.Id;

            if (tipToDelete != null) {
                tipRepository.Delete(tipToDelete);

                try {
                    tipRepository.DbContext.CommitChanges();
                }
                catch {
                    resultMessage = "По ряду причин подскакза не может быть удалена. Возможно, она нужна где-то еще.";
                    tipRepository.DbContext.RollbackTransaction();
                }
            }
            else {
                resultMessage = "Удаляемая подсказка не найдена. Возможно, ее уже удалили.";
            }

            Message = resultMessage;
            return this.RedirectToAction<TasksController>(c => c.Edit(taskId));
        }

		/// <summary>
		/// Holds data to be passed to the Tip form for creates and edits
		/// </summary>
        public class TipFormViewModel
        {
            private TipFormViewModel() { }

			/// <summary>
			/// Creation method for creating the view model. Services may be passed to the creation 
			/// method to instantiate items such as lists for drop down boxes.
			/// </summary>
            public static TipFormViewModel CreateTipFormViewModel() {
                TipFormViewModel viewModel = new TipFormViewModel();
                
                return viewModel;
            }

            public Tip Tip { get; internal set; }
            public int TaskId { get; internal set; }
        }

        private readonly IRepository<Tip> tipRepository;
        private readonly IRepository<Task> taskRepository;
    }
}
