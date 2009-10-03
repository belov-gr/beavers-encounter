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
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class LockIfGameStartAttribute : FilterUsingAttribute
    {
        public LockIfGameStartAttribute()
            : base(typeof(LockIfGameStartFilter))
        {
            Order = 11;
        }
    }

    public class LockIfGameStartFilter : IActionFilter
    {
        private IRepository<Game> gameRepository;

        public LockIfGameStartFilter(IRepository<Game> gameRepository)
        {
            Check.Require(gameRepository != null, "gameRepository may not be null");

            this.gameRepository = gameRepository;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Если игра запушена и пользователь не усаствует в игре, 
            // то позволяем ему доступ только к главной странице
            bool gameIsRun = gameRepository.GetAll().Any(
                g => g.GameState == (int) GameStates.Startup ||
                     g.GameState == (int) GameStates.Started ||
                     g.GameState == (int) GameStates.Finished);
            User user = (User)filterContext.HttpContext.User;
            if ((user.Role.IsPlayer || user.Role.IsTeamLeader) && user.Team != null && user.Team.TeamGameState == null && gameIsRun)
            {
                filterContext.Result = new RedirectResult("/Home");
            }
        }
    }
}
