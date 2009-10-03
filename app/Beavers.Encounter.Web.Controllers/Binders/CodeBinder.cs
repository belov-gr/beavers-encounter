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
    public class CodeBinderAttribute : CustomModelBinderAttribute
    {
        public CodeBinderAttribute()
        {
            Fetch = true;
        }

        public override IModelBinder GetBinder()
        {
            return new CodeBinder(
                ServiceLocator.Current.GetInstance(typeof(IRepository<Code>)) as IRepository<Code>,
                ServiceLocator.Current.GetInstance(typeof(IRepository<Task>)) as IRepository<Task>,
                Fetch);
        }

        public bool Fetch { get; set; }
    }

    public class CodeBinder : IModelBinder
    {
        private bool fetch;
        private readonly IRepository<Task> taskRepository;
        private readonly IRepository<Code> codeRepository;

        public CodeBinder(IRepository<Code> codeRepository, IRepository<Task> taskRepository, bool fetch)
        {
            Check.Require(codeRepository != null, "codeRepository may not be null");
            Check.Require(taskRepository != null, "taskRepository may not be null");

            this.codeRepository = codeRepository;
            this.taskRepository = taskRepository;
            this.fetch = fetch;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            NameValueCollection values = controllerContext.HttpContext.Request.Form;
            Code code = fetch ? codeRepository.Get(Convert.ToInt32(values["code.Id"])) : new Code();

            code.Name = values["code.Name"];
            code.Danger = values["code.Danger"];
            code.IsBonus = Convert.ToBoolean(values["code.IsBonus"].Split(new char[] { ',' })[0]) ? 1 : 0;

            if (!fetch)
            {
                code.Task = taskRepository.Get(Convert.ToInt32(values["taskId"]));
            }
            return code;
        }
    }
}
