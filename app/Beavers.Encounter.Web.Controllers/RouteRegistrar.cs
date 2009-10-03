using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Web.Areas;

namespace Beavers.Encounter.Web.Controllers
{
    public class RouteRegistrar
    {
        public static void RegisterRoutesTo(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            // The areas below must be registered from greater subareas to fewer;
            // i.e., the root area should be the last area registered

            // Example illustrative routes with a nested area - note that the order of registration is important
            //routes.CreateArea("Organization/Department", "Beavers.Encounter.Web.Controllers.Organization.Department",
            //    routes.MapRoute(null, "Organization/Department/{controller}/{action}", new { action = "Index" }),
            //    routes.MapRoute(null, "Organization/Department/{controller}/{action}/{id}")
            //);
            //routes.CreateArea("Organization", "Beavers.Encounter.Web.Controllers.Organization",
            //    routes.MapRoute(null, "Organization/{controller}/{action}", new { action = "Index" }),
            //    routes.MapRoute(null, "Organization/{controller}/{action}/{id}")
            //);

            //routes.MapRoute(
            //    "Default",
            //    "{controller}.mvc/{action}/{id}",
            //    new { action = "Index", id = "" }
            //);

            //routes.MapRoute(
            //    "Root",
            //    "",
            //    new { controller = "Home", action = "Index", id = "" }
            //);
            
            // Routing config for the root area
            routes.CreateArea("Root", "Beavers.Encounter.Web.Controllers",
                routes.MapRoute(null, "{controller}/{action}", new { controller = "Home", action = "Index" }),
                routes.MapRoute(null, "{controller}/{action}/{id}")
            );
        }
    }
}
