using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using Beavers.Encounter.Core;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Binders
{
    public class BonusTaskBinderAttribute : CustomModelBinderAttribute
    {
        public BonusTaskBinderAttribute()
        {
            Fetch = true;
        }

        public override IModelBinder GetBinder()
        {
            return new BonusTaskBinder(
                ServiceLocator.Current.GetInstance(typeof(IRepository<BonusTask>)) as IRepository<BonusTask>,
                Fetch);
        }

        public bool Fetch { get; set; }
    }

    public class BonusTaskBinder : IModelBinder
    {
        private bool fetch;
        private readonly IRepository<BonusTask> taskRepository;

        public BonusTaskBinder(IRepository<BonusTask> taskRepository, bool fetch)
        {
            Check.Require(taskRepository != null, "taskRepository may not be null");

            this.taskRepository = taskRepository;
            this.fetch = fetch;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection values = controllerContext.HttpContext.Request.Form;
            BonusTask task = fetch ? taskRepository.Get(Convert.ToInt32(values["BonusTask.Id"])) : new BonusTask();

            task.Name = values["BonusTask.Name"];
            task.TaskText = values["BonusTask.TaskText"];
            task.StartTime = Convert.ToDateTime(values["BonusTask.StartTime"]);
            task.FinishTime = Convert.ToDateTime(values["BonusTask.FinishTime"]);
            task.IsIndividual = Convert.ToBoolean(values["BonusTask.IsIndividual"].Split(new char[] { ',' })[0]);

            return task;
        }
    }
}
