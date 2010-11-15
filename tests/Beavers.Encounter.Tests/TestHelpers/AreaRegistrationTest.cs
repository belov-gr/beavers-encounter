using System.Web.Mvc;
using System.Web.Routing;

namespace Tests.TestHelpers
{
    public static class AreaRegistrationTest
    {
        public static void RegisterAllAreas(RouteCollection routes, AreaRegistration[] areas)
        {
            foreach (AreaRegistration area in areas)
            {
                var context = new AreaRegistrationContext(area.AreaName, routes);
                context.Namespaces.Add(area.GetType().Namespace);
                area.RegisterArea(context);
            }
        }

        public static RouteData AreaRoute(this string url, RouteCollection routes)
        {
            return routes.GetRouteData(new TestHttpContext(url));
        }
    }
}
