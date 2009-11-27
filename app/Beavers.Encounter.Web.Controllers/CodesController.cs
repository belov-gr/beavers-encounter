using System.Web.Mvc;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Web.Controllers.Binders;
using Beavers.Encounter.Web.Controllers.Filters;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;
using SharpArch.Core;
using Beavers.Encounter.Common.MvcContrib;

namespace Beavers.Encounter.Web.Controllers
{
    [CodeOwner]
    [HandleError]
    public class CodesController : BaseController
    {
        public CodesController(IRepository<Code> codeRepository, IRepository<Task> taskRepository, IUserRepository userRepository)
            : base(userRepository)
        {
            Check.Require(codeRepository != null, "codeRepository may not be null");
            Check.Require(taskRepository != null, "taskRepository may not be null");

            this.codeRepository = codeRepository;
            this.taskRepository = taskRepository;
        }

        public ActionResult Create(int taskId)
        {
            CodeFormViewModel viewModel = CodeFormViewModel.CreateCodeFormViewModel();
            viewModel.Code = new Code { Name = "New code", Danger = "1" };
            viewModel.TaskId = taskId;
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([CodeBinder(Fetch = false)]Code code)
        {
            if (ViewData.ModelState.IsValid && code.IsValid()) 
            {
                taskRepository.Get(code.Task.Id).Codes.Add(code);
                codeRepository.SaveOrUpdate(code);
                taskRepository.DbContext.CommitChanges();

                Message = "The code was successfully created.";
                return this.RedirectToAction<TasksController>(c => c.Edit(code.Task.Id));
            }

            CodeFormViewModel viewModel = CodeFormViewModel.CreateCodeFormViewModel();
            viewModel.Code = code;
            return View(viewModel);
        }

        [Transaction]
        public ActionResult Edit(int id) {
            CodeFormViewModel viewModel = CodeFormViewModel.CreateCodeFormViewModel();
            viewModel.Code = codeRepository.Get(id);
            viewModel.TaskId = viewModel.Code.Task.Id;
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit([CodeBinder(Fetch = true)]Code code)
        {
            Code codeToUpdate = codeRepository.Get(code.Id);
            TransferFormValuesTo(codeToUpdate, code);

            if (ViewData.ModelState.IsValid && code.IsValid()) {
                Message = "The code was successfully updated.";
                return this.RedirectToAction<TasksController>(c => c.Edit(code.Task.Id));
            }
            else {
                codeRepository.DbContext.RollbackTransaction();

				CodeFormViewModel viewModel = CodeFormViewModel.CreateCodeFormViewModel();
				viewModel.Code = code;
				return View(viewModel);
            }
        }

        private void TransferFormValuesTo(Code codeToUpdate, Code codeFromForm) {
			codeToUpdate.Name = codeFromForm.Name;
			codeToUpdate.Danger = codeFromForm.Danger;
            codeToUpdate.IsBonus = codeFromForm.IsBonus;
            codeToUpdate.Task = codeFromForm.Task;
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id) {
            string resultMessage = "The code was successfully deleted.";
            Code codeToDelete = codeRepository.Get(id);
            int taskId = codeToDelete.Task.Id;

            if (codeToDelete != null) {
                codeRepository.Delete(codeToDelete);

                try {
                    codeRepository.DbContext.CommitChanges();
                }
                catch {
                    resultMessage = "A problem was encountered preventing the code from being deleted. " +
						"Another item likely depends on this code.";
                    codeRepository.DbContext.RollbackTransaction();
                }
            }
            else {
                resultMessage = "The code could not be found for deletion. It may already have been deleted.";
            }

            Message = resultMessage;
            return this.RedirectToAction<TasksController>(c => c.Edit(taskId));
        }

		/// <summary>
		/// Holds data to be passed to the Code form for creates and edits
		/// </summary>
        public class CodeFormViewModel
        {
            private CodeFormViewModel() { }

			/// <summary>
			/// Creation method for creating the view model. Services may be passed to the creation 
			/// method to instantiate items such as lists for drop down boxes.
			/// </summary>
            public static CodeFormViewModel CreateCodeFormViewModel() {
                CodeFormViewModel viewModel = new CodeFormViewModel();
                
                return viewModel;
            }

            public Code Code { get; internal set; }
            public int TaskId { get; internal set; }
        }

        private readonly IRepository<Code> codeRepository;
        private readonly IRepository<Task> taskRepository;
    }
}
