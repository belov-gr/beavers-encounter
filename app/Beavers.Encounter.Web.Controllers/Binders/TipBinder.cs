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
    public class TipBinderAttribute : CustomModelBinderAttribute
    {
        public TipBinderAttribute()
        {
            Fetch = true;
        }

        public override IModelBinder GetBinder()
        {
            return new TipBinder(
                ServiceLocator.Current.GetInstance(typeof(IRepository<Tip>)) as IRepository<Tip>, 
                ServiceLocator.Current.GetInstance(typeof(IRepository<Task>)) as IRepository<Task>, 
                Fetch);
        }

        public bool Fetch { get; set; }
    }

    public class TipBinder : IModelBinder
    {
        private bool fetch;
        private readonly IRepository<Task> taskRepository;
        private readonly IRepository<Tip> tipRepository;

        public TipBinder(IRepository<Tip> tipRepository, IRepository<Task> taskRepository, bool fetch)
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
            Tip tip = fetch ? tipRepository.Get(Convert.ToInt32(values["tip.Id"])) : new Tip();
            
            tip.Name = values["tip.Name"];
            tip.SuspendTime = Convert.ToInt32(values["tip.SuspendTime"]);
            
            if (!fetch)
            {
                tip.Task = taskRepository.Get(Convert.ToInt32(values["taskId"]));
            }
            return tip;
        }
    }
}
