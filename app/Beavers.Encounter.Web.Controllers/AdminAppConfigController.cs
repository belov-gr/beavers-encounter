using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;

using Beavers.Encounter.Common;
using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;

namespace Beavers.Encounter.Web.Controllers
{
    [AdministratorsOnly]
    public class AdminAppConfigController : BaseController
    {
        private readonly IRepository<AppConfig> appConfigRepository;

        public AdminAppConfigController(IUserRepository userRepository,
            IRepository<AppConfig> appConfigRepository)
            : base(userRepository)
        {
            Check.Require(appConfigRepository != null, "appConfigRepository may not be null");

            this.appConfigRepository = appConfigRepository;
        }

        [Breadcrumb("Настройки сайта", 3)]
        public ActionResult Edit()
        {
            var appConfig = appConfigRepository.Get(1);
            return View(appConfig);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(AppConfig appConfig)
        {
            var gameToUpdate = appConfigRepository.Get(1);

            gameToUpdate.Title = appConfig.Title;

            if (ViewData.ModelState.IsValid && appConfig.IsValid())
            {
                Message = "Настройки успешно сохранены.";
                return this.RedirectToAction(c => c.Edit());
            }

            appConfigRepository.DbContext.RollbackTransaction();
            return Edit(appConfig);
        }
    }
}
