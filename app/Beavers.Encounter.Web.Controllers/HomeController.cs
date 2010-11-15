using System.Web.Mvc;
using Beavers.Encounter.Common;
using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Web.Controllers.Filters;

namespace Beavers.Encounter.Web.Controllers
{
    [GameState]
    [HandleError]
    public class HomeController : BaseController
    {
        public HomeController(IUserRepository userRepository)
            : base(userRepository)
        {
        }

        [Breadcrumb("Главная", 1)]
        public ActionResult Index()
        {
            if (User.IsAdministrator)
            {
                return this.RedirectToAction<AdminController>(c => c.Index());
            }

            return View();
        }
    }
}
