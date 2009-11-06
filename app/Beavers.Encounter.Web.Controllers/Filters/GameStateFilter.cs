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
using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class GameStateAttribute : FilterUsingAttribute
    {
        public GameStateAttribute()
            : base(typeof(GameStateFilter))
        {
            Order = 10;
        }
    }

    public class GameStateFilter : IActionFilter
    {
        public GameStateFilter()
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            User user = (User) filterContext.HttpContext.User;
            if (user.Team != null &&
                (user.Role.IsPlayer || user.Role.IsTeamLeader) && 
                user.Team.Game != null && 
                user.Team.Game.GameState != GameStates.Planned &&
                user.Team.TeamGameState != null)
            {
                filterContext.Result = new RedirectResult("/TeamGameboard/Show");
            }
        }
    }
}
