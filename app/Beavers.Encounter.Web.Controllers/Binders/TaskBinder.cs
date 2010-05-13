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
                ServiceLocator.Current.GetInstance(typeof(IRepository<Team>)) as IRepository<Team>,
                ServiceLocator.Current.GetInstance(typeof(IRepository<Task>)) as IRepository<Task>,
                Fetch);
        }

        public bool Fetch { get; set; }
    }

    public class TaskBinder : IModelBinder
    {
        private bool fetch;
        private readonly IRepository<Task> taskRepository;
        private readonly IRepository<Team> teamRepository;

        public TaskBinder(IRepository<Team> teamRepository, IRepository<Task> taskRepository, bool fetch)
        {
            Check.Require(teamRepository != null, "teamRepository may not be null");
            Check.Require(taskRepository != null, "taskRepository may not be null");

            this.teamRepository = teamRepository;
            this.taskRepository = taskRepository;
            this.fetch = fetch;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection values = controllerContext.HttpContext.Request.Form;
            
            if (values.AllKeys.Contains("id"))
                return BindCooliteModel(controllerContext, bindingContext);

            Task task = fetch ? taskRepository.Get(Convert.ToInt32(values["Task.Id"])) : new Task();

            task.Name = values["Task.Name"];
            task.StreetChallendge = Convert.ToBoolean(values["Task.StreetChallendge"].Split(new char[] {','})[0]);
            task.Agents = Convert.ToBoolean(values["Task.Agents"].Split(new char[] { ',' })[0]);
            task.Locked = Convert.ToBoolean(values["Task.Locked"].Split(new char[] { ',' })[0]);
            task.TaskType = (TaskTypes)Convert.ToInt32(values["Task.TaskType"]);
            task.Priority = Convert.ToInt32(values["Task.Priority"]);
            task.GiveTaskAfter = (GiveTaskAfter)Convert.ToInt32(values["Task.GiveTaskAfter"]);

            int afterTaskId = Convert.ToInt32(values["Task.AfterTask"]);
            task.AfterTask = taskRepository.Get(afterTaskId);

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

            // ----------------------------------
            // Не выдавать задание
            var forRemoveTeams = new System.Collections.Generic.List<Team>();

            for (int i = 0; i < 5; i++)
            {
                if (values.AllKeys.Contains("Task.NotForTeams" + i))
                {
                    int notForTeamTaskId = Convert.ToInt32(values["Task.NotForTeams" + i]);
                    if (notForTeamTaskId != 0)
                    {
                        var t = teamRepository.Get(notForTeamTaskId);
                        if (!task.NotForTeams.Contains(t))
                            task.NotForTeams.Add(t);
                    }
                    else
                    {
                        try
                        {
                            forRemoveTeams.Add(task.NotForTeams[i]);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                        }
                    }
                }
            }

            foreach (var item in forRemoveTeams)
            {
                task.NotForTeams.Remove(item);
            }

            if (!fetch)
            {
                //task.Game = taskRepository.Get(Convert.ToInt32(values["taskId"]));
            }
            return task;
        }
        public object BindCooliteModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection values = controllerContext.HttpContext.Request.Form;
            fetch = !String.IsNullOrEmpty(values["id"]);
            Task task = fetch ? taskRepository.Get(Convert.ToInt32(values["Task_Id"])) : new Task();

            task.Name = values["Task_Name"];
            task.StreetChallendge = values["Task_StreetChallendge"] != null;
            task.Agents = values["Task_Agents"] != null;
            task.Locked = values["Task_Locked"] != null;
            task.TaskType = (TaskTypes)Enum.Parse(typeof(TaskTypes), values["Task_TaskType_Value"]);
            task.Priority = Convert.ToInt32(values["Task_Priority"]);

            // ----------------------------------
            // Не после
            string[] notAfterTasksIds = values["Task_NotAfterTasks"].Split(new char[] { ',' });
            task.NotAfterTasks.Clear();
            foreach (string strId in notAfterTasksIds)
            {
                if (String.IsNullOrEmpty(strId))
                    continue;
                var t = taskRepository.Get(Convert.ToInt32(strId));
                if (!task.NotAfterTasks.Contains(t))
                    task.NotAfterTasks.Add(t);
            }

            // ----------------------------------
            // Не вместе
            string[] notOneTimeTasksIds = values["Task_NotOneTimeTasks"].Split(new char[] { ',' });
            task.NotOneTimeTasks.Clear();
            foreach (string strId in notOneTimeTasksIds)
            {
                if (String.IsNullOrEmpty(strId))
                    continue;
                var t = taskRepository.Get(Convert.ToInt32(strId));
                if (!task.NotOneTimeTasks.Contains(t))
                    task.NotOneTimeTasks.Add(t);
            }

            return task;
        }
    }
}
