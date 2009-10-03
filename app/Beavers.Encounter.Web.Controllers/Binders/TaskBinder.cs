using System;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Beavers.Encounter.Core;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Binders
{
    public class TaskBinderAttribute : CustomModelBinderAttribute
    {
        public TaskBinderAttribute()
        {
            Fetch = true;
        }

        public override IModelBinder GetBinder()
        {
            return new TaskBinder(
                ServiceLocator.Current.GetInstance(typeof(IRepository<Tip>)) as IRepository<Tip>,
                ServiceLocator.Current.GetInstance(typeof(IRepository<Task>)) as IRepository<Task>,
                Fetch);
        }

        public bool Fetch { get; set; }
    }

    public class TaskBinder : IModelBinder
    {
        private bool fetch;
        private readonly IRepository<Task> taskRepository;
        private readonly IRepository<Tip> tipRepository;

        public TaskBinder(IRepository<Tip> tipRepository, IRepository<Task> taskRepository, bool fetch)
        {
            Check.Require(tipRepository != null, "tipRepository may not be null");
            Check.Require(taskRepository != null, "taskRepository may not be null");

            this.tipRepository = tipRepository;
            this.taskRepository = taskRepository;
            this.fetch = fetch;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection values = controllerContext.HttpContext.Request.Form;
            Task task = fetch ? taskRepository.Get(Convert.ToInt32(values["Task.Id"])) : new Task();

            task.Name = values["Task.Name"];
            task.StreetChallendge = Convert.ToBoolean(values["Task.StreetChallendge"].Split(new char[] {','})[0]) ? 1 : 0;
            task.Agents = Convert.ToBoolean(values["Task.Agents"].Split(new char[] { ',' })[0]) ? 1 : 0;
            task.Locked = Convert.ToBoolean(values["Task.Locked"].Split(new char[] { ',' })[0]) ? 1 : 0;

            // ----------------------------------
            // Не после
            var forRemove = new System.Collections.Generic.List<Task>();

            for (int i = 0; i < 5; i++)
            {
                if (values.AllKeys.Contains("Task.NotAfterTasks" + i))
                {
                    int notAfterTaskId = Convert.ToInt32(values["Task.NotAfterTasks" + i]);
                    if (notAfterTaskId != 0)
                    {
                        var t = taskRepository.Get(notAfterTaskId);
                        if (!task.NotAfterTasks.Contains(t))
                            task.NotAfterTasks.Add(t);
                    }
                    else
                    {
                        try
                        {
                            forRemove.Add(task.NotAfterTasks[i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                }
            }

            foreach (var item in forRemove)
            {
                task.NotAfterTasks.Remove(item);
            }

            // ----------------------------------
            // Не вместе

            forRemove = new System.Collections.Generic.List<Task>();

            for (int i = 0; i < 5; i++)
            {
                if (values.AllKeys.Contains("Task.NotOneTimeTasks" + i))
                {
                    int notOneTimeTaskId = Convert.ToInt32(values["Task.NotOneTimeTasks" + i]);
                    if (notOneTimeTaskId != 0)
                    {
                        var t = taskRepository.Get(notOneTimeTaskId);
                        if (!task.NotOneTimeTasks.Contains(t))
                            task.NotOneTimeTasks.Add(t);
                    }
                    else
                    {
                        try
                        {
                            forRemove.Add(task.NotOneTimeTasks[i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                }
            }

            foreach (var item in forRemove)
            {
                task.NotOneTimeTasks.Remove(item);
            }

            if (!fetch)
            {
                //task.Game = taskRepository.Get(Convert.ToInt32(values["taskId"]));
            }
            return task;
        }
    }
}
