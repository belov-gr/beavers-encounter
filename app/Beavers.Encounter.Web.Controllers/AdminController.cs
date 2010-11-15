using System.Web.Mvc;
using Beavers.Encounter.Common;
using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core.DataInterfaces;

namespace Beavers.Encounter.Web.Controllers
{
    [AdministratorsOnly]
    public class AdminController : BaseController
    {
        public AdminController(IUserRepository userRepository)
            : base(userRepository)
        {}

        [Breadcrumb("Администрирование", 2)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
