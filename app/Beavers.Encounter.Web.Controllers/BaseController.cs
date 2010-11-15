using System.Web.Mvc;
using SharpArch.Core;
using MvcContrib.Filters;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Web.Controllers.Filters;

namespace Beavers.Encounter.Web.Controllers
{

    [Rescue("Default"), Authenticate /*, CopyMessageFromTempDataToViewData*/]
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
