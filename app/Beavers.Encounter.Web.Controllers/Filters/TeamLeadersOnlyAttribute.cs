using System.Web.Mvc;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class TeamLeadersOnlyAttribute : AuthorizeAttribute
    {
        public TeamLeadersOnlyAttribute()
        {
            Roles = "TeamLeader";
            Order = 1; //Must come AFTER AuthenticateAttribute
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.Request.Path.StartsWith("/Teams/"))
            {
                int id = System.Convert.ToInt32(filterContext.HttpContext.Request.QueryString["id"]);
                if (filterContext.HttpContext.User is Core.User && 
                    ((Core.User)filterContext.HttpContext.User).Team != null &&
                    ((Core.User)filterContext.HttpContext.User).Team.Id == id)
                {
                    // OK                    
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Home");
                }
            }
        }
    }
}
