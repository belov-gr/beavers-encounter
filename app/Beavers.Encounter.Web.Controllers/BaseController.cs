using System;
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
using Beavers.Encounter.Core.DataInterfaces;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Core;
using Beavers.Encounter.Web.Controllers.Filters;
using MvcContrib.Filters;

namespace Beavers.Encounter.Web.Controllers
{
    [CompressionFilter(Order = 1)]
    [Rescue("Default"), Authenticate/*, CopyMessageFromTempDataToViewData*/]
    public class BaseController : Controller
    {
        protected IUserRepository UserRepository { get; set; }

        public BaseController(IUserRepository userRepository)
        {
            Check.Require(userRepository != null, "userRepository may not be null");

            UserRepository = userRepository;
        }

        public new User User
        {
            get { return (User)base.User; }
        }

        public virtual void AppendMetaDescription(string text)
        {
            ViewData["MetaDescription"] = text;
        }

        public string Message
        {
            get { return TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] as string; }
            set { TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()] = value; }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Response.Clear();
            base.OnException(filterContext);
        }
    }
}
