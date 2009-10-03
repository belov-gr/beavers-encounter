using System.Web.Mvc;
using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class TeamGameboardAttribute : FilterUsingAttribute
    {
        public TeamGameboardAttribute()
            : base(typeof(TeamGameboardFilter))
        {
            Order = 1;
        }
    }

    public class TeamGameboardFilter : IActionFilter
    {
        #region IActionFilter Members

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User user = (User)filterContext.HttpContext.User;
            if (user.Team == null || user.Team.Game == null)
            {
                filterContext.Result = new RedirectResult("/Home");
            }
        }

        #endregion
    }
}
