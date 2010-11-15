using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using MvcContrib.TestHelper;
using Beavers.Encounter.Web.Controllers;
using Tests.TestHelpers;

namespace Tests.Beavers.Encounter.Web.Controllers.Admin
{
    [TestFixture]
    public class RouteRegistrarTests
    {
        private RouteCollection routeConfig;

        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();
            RouteRegistrar.RegisterRoutesTo(RouteTable.Routes);
        }

        [Test]
        public void CanVerifyDefaultRouteMaps()
        {
            "~/Admin".AreaRoute(RouteTable.Routes).ShouldMapTo<AdminController>(x => x.Index());
        }

        [Test]
        public void CanVerifyUsersRouteMaps()
        {
            "~/AdminUsers".AreaRoute(RouteTable.Routes).ShouldMapTo<AdminUsersController>(x => x.Index());
        }

        [Test]
        public void CanVerifyUsersIndexRouteMaps()
        {
            "~/AdminUsers/Index".AreaRoute(RouteTable.Routes).ShouldMapTo<AdminUsersController>(x => x.Index());
        }
    }
}
