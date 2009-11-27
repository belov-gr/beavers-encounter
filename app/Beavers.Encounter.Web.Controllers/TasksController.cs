using System.Web.Mvc;
using Beavers.Encounter.Web.Controllers.Binders;
using Beavers.Encounter.Web.Controllers.Filters;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;

using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;


namespace Beavers.Encounter.Web.Controllers
{
    [TaskOwner]
    [HandleError]
    public class TasksController : BaseController
    {
        public TasksController(ITaskRepository taskRepository, IUserRepository userRepository, IRepository<Tip> tipsRepository, IRepository<Code> codesRepository)
            : base(userRepository)
        {
            Check.Require(taskRepository != null, "taskRepository may not be null");
            Check.Require(tipsRepository != null, "tipsRepository may not be null");
            Check.Require(codesRepository != null, "codesRepository may not be null");

            this.taskRepository = taskRepository;
            this.tipsRepository = tipsRepository;
            this.codesRepository = codesRepository;
        }

        [Transaction]
        public ActionResult Show(int id) {
            Task task = taskRepository.Get(id);
            return View(task);
        }

        public ActionResult Create() {
            TaskFormViewModel viewModel = TaskFormViewModel.CreateTaskFormViewModel();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([TaskBinder(Fetch = false)] Task task) 
        {
            if (ViewData.ModelState.IsValid && task.IsValid()) 
            {
                task.Game = User.Game;
                
                Tip tip = new Tip();
                tip.Name = "Здесь должен быть текст задания...";
                tip.SuspendTime = 0;
                tip.Task = task;
                task.Tips.Add(tip);

                taskRepository.SaveOrUpdate(task);
                tipsRepository.SaveOrUpdate(tip);

                Message = "The task was successfully created.";

                return this.RedirectToAction<GamesController>(c => c.Edit(task.Game.Id));
            }

            TaskFormViewModel viewModel = TaskFormViewModel.CreateTaskFormViewModel();
            viewModel.Task = task;
            return View(viewModel);
        }

        [Transaction]
        public ActionResult Edit(int id) {
            TaskFormViewModel viewModel = TaskFormViewModel.CreateTaskFormViewModel();
            viewModel.Task = taskRepository.Get(id);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([TaskBinder(Fetch = true)] Task task)
        {
            Task taskToUpdate = taskRepository.Get(task.Id);
            TransferFormValuesTo(taskToUpdate, task);

            if (ViewData.ModelState.IsValid && task.IsValid()) {
                Message = "The task was successfully updated.";
                return this.RedirectToAction<GamesController>(c => c.Edit(taskToUpdate.Game.Id));
            }
            else {
                taskRepository.DbContext.RollbackTransaction();

				TaskFormViewModel viewModel = TaskFormViewModel.CreateTaskFormViewModel();
				viewModel.Task = task;
				return View(viewModel);
            }
        }

        private void TransferFormValuesTo(Task taskToUpdate, Task taskFromForm) {
			taskToUpdate.Name = taskFromForm.Name;
            taskToUpdate.StreetChallendge = taskFromForm.StreetChallendge;
            taskToUpdate.Agents = taskFromForm.Agents;
            taskToUpdate.Locked = taskFromForm.Locked;
            taskToUpdate.TaskType = taskFromForm.TaskType;
            taskToUpdate.Priority = taskFromForm.Priority;
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            string resultMessage = "The task was successfully deleted.";
            Task taskToDelete = taskRepository.Get(id);
            int gameId = taskToDelete.Game.Id;

            if (taskToDelete != null) 
            {
                taskRepository.Delete(taskToDelete);

                try 
                {
                    taskRepository.DbContext.CommitChanges();
                }
                catch 
                {
                    resultMessage = "A problem was encountered preventing the task from being deleted. " +
						"Another item likely depends on this task.";
                    taskRepository.DbContext.RollbackTransaction();
                }
            }
            else 
            {
                resultMessage = "The task could not be found for deletion. It may already have been deleted.";
            }

            Message = resultMessage;
            return this.RedirectToAction<GamesController>(c => c.Edit(gameId));
        }

		/// <summary>
		/// Holds data to be passed to the Task form for creates and edits
		/// </summary>
        public class TaskFormViewModel
        {
            private TaskFormViewModel() { }

			/// <summary>
			/// Creation method for creating the view model. Services may be passed to the creation 
			/// method to instantiate items such as lists for drop down boxes.
			/// </summary>
            public static TaskFormViewModel CreateTaskFormViewModel() {
                TaskFormViewModel viewModel = new TaskFormViewModel();
                
                return viewModel;
            }

            public Task Task { get; internal set; }
        }

        private readonly ITaskRepository taskRepository;
        private readonly IRepository<Tip> tipsRepository;
        private readonly IRepository<Code> codesRepository;
    }
}
