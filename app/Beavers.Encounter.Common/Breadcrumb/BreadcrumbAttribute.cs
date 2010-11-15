using System;
using System.Web.Mvc;

namespace Beavers.Encounter.Common
{
    public class BreadcrumbAttribute : ActionFilterAttribute
    {
        public string Title { get; set; }
        public int Level { get; set; }

        private string breadcrumbItemTitle;

        public BreadcrumbAttribute(string title, int level)
        {
            Title = title;
            Level = level;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var controller = (Controller)filterContext.Controller;

            string breadcrumbText = controller.GetBreadcrumbText();

            if (!String.IsNullOrEmpty(Title))
            {
                breadcrumbItemTitle = String.Format(Title, breadcrumbText);
            }

            Breadcrumb[] breadcrumbs = GetBreadcrumbManager(filterContext)
                .PushBreadcrumb(
                    filterContext.HttpContext.Request.RawUrl,
                    breadcrumbItemTitle,
                    Level);
            
            controller.ViewData.Add("breadcrumbs", breadcrumbs);

            base.OnResultExecuting(filterContext);
        }

        private static IBreadcrumbManager GetBreadcrumbManager(ResultExecutingContext filterContext)
        {
            var bcm = filterContext.HttpContext.Session["breadcrumbManager"] as IBreadcrumbManager;
            if (bcm == null)
            {
                bcm = new BreadcrumbManager();
                filterContext.HttpContext.Session["breadcrumbManager"] = bcm;
            }
            return bcm;
        }
    }
}
