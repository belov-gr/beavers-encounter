using System.Web.Mvc;
using log4net;

namespace Beavers.Encounter.Common.Filters
{
    public class AuthorsOnlyAttribute : AuthorizeAttribute
    {
        public AuthorsOnlyAttribute()
        {
            Roles = "Author";
            Order = 1; //Must come AFTER AuthenticateAttribute
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.UserHostAddress == "127.0.0.1" ||
                filterContext.RequestContext.HttpContext.Request.UserHostAddress == "::1")
            {
                filterContext.Result = null;
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}
