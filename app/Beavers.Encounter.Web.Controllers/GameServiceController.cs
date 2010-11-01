using System;
using System.Data;
using System.IO;
using System.Web.Mvc;
using Beavers.Encounter.ApplicationServices;
using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core.DataInterfaces;
using SharpArch.Core;

namespace Beavers.Encounter.Web.Controllers
{
    [HandleError]
    public class GameServiceController : BaseController
    {
        private readonly IGameReportService gameReportService;
        private readonly IGameService gameService;

        public GameServiceController(IUserRepository userRepository, IGameService gameService, IGameReportService gameReportService)
            : base(userRepository)
        {
            Check.Require(gameService != null, "gameService may not be null");
            Check.Require(gameReportService != null, "gameReportService may not be null");

            this.gameService = gameService;
            this.gameReportService = gameReportService;
        }

        [AuthorsOnly]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetState(int id)
        {
            Stream reportStream = gameReportService.GetReport(User);

            return File(reportStream, "application/zip", String.Format("report_{0}.zip", DateTime.Now.TimeOfDay).Replace(":", "-"));
        }

        [AuthorsOnly]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Results(int id)
        {
            DataRow[] report = gameService.GetGameResults(id).Select("", "tasks desc, bonus desc, time asc");

            return View(report);
        }

        /// <summary>
        /// Holds data to be passed to the Task form for creates and edits
        /// </summary>
        public class GameServiceViewModel
        {
            private GameServiceViewModel() { }

            /// <summary>
            /// Creation method for creating the view model. Services may be passed to the creation 
            /// method to instantiate items such as lists for drop down boxes.
            /// </summary>
            public static GameServiceViewModel CreateGameServiceViewModel()
            {
                return new GameServiceViewModel();
            }

            public int GameId { get; internal set; }
        }
    }
}
