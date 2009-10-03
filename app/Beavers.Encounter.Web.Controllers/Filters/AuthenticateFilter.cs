using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Beavers.Encounter.ApplicationServices;
using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class AuthenticateAttribute : FilterUsingAttribute
    {
        public AuthenticateAttribute()
            : base(typeof(AuthenticateFilter))
        {
            Order = 0;
        }
    }

    public class AuthenticateFilter : IAuthorizationFilter
    {
        private IRepository<User> userRepository;
        private IFormsAuthentication formsAuth;
        private log4net.ILog log;

        public AuthenticateFilter(IRepository<User> userRepository, IFormsAuthentication formsAuth)
        {
            this.userRepository = userRepository;
            this.formsAuth = formsAuth;
            log = log4net.LogManager.GetLogger("LogToFile");
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var context = filterContext.HttpContext;

            try
            {
                log.Warn(String.Format("OnAuthorization {0}, {1}, {2}, {3}, {4}, {5}",
                                       filterContext.Controller.GetType().Name,
                                       filterContext.HttpContext.User.Identity.Name,
                                       filterContext.HttpContext.Request.Path,
                                       filterContext.HttpContext.Request.UserHostName,
                                       filterContext.HttpContext.Request.UserAgent,
                                       filterContext.HttpContext.Request.Headers["Accept-Charset"]
                                       ));
            }
            catch(Exception)
            {}

            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var login = context.User.Identity.Name;
                var user = userRepository.GetAll().WhereLoginIs(login);

                if (user == null)
                {
                    formsAuth.SignOut();
                }
                else
                {
                    AuthenticateAs(context, user);
                    return;
                }
            }

            AuthenticateAs(context, User.Guest);
        }

        private void AuthenticateAs(HttpContextBase context, User user)
        {
            Thread.CurrentPrincipal = context.User = user;
        }
    }
}
