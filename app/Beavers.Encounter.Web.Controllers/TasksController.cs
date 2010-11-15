using System.Web.Mvc;
using Beavers.Encounter.Common;
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

        [Breadcrumb("Задание \"{0}\"", 4)]
        public ActionResult Show(int id)
        {
            Task task = taskRepository.Get(id);
            this.SetBreadcrumbText(task.Name);
            return View(task);
        }

        [Breadcrumb("Новое задание", 4)]
        public ActionResult Create()
        {
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
                task.GroupTag = 0;
                
                Tip tip = new Tip();
                tip.Name = "Здесь должен быть текст задания...";
                tip.SuspendTime = 0;
                tip.Task = task;
                task.Tips.Add(tip);

                taskRepository.SaveOrUpdate(task);
                tipsRepository.SaveOrUpdate(tip);

                Message = "Задание успешно создано.";

                return this.RedirectToAction(c => c.Edit(task.Id));
            }

            TaskFormViewModel viewModel = TaskFormViewModel.CreateTaskFormViewModel();
            viewModel.Task = task;
            return View(viewModel);
        }

        [Breadcrumb("Задание \"{0}\"", 4)]
        public ActionResult Edit(int id)
        {
            TaskFormViewModel viewModel = TaskFormViewModel.CreateTaskFormViewModel();
            viewModel.Task = taskRepository.Get(id);
            this.SetBreadcrumbText(viewModel.Task.Name);
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
                Message = "Задание успешно изменено.";
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
            taskToUpdate.GroupTag = taskFromForm.GroupTag;
            taskToUpdate.AfterTask = taskFromForm.AfterTask;
            taskToUpdate.GiveTaskAfter = taskFromForm.GiveTaskAfter;
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            string resultMessage = "Задание успешно удалено.";
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
                    resultMessage = "По ряду причин задание не может быть удалено. Возможно, оно еще где-то нужно.";
                    taskRepository.DbContext.RollbackTransaction();
                }
            }
            else 
            {
                resultMessage = "Удаляемое задание не обнаружено. Возможно, его уже удалили.";
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
