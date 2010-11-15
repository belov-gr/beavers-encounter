using System;
using System.Web.Mvc;

namespace Beavers.Encounter.Common
{
    public static class BreadcrumbHelper
    {
        public static void SetBreadcrumbText(this Controller controller, string breadcrumbText)
        {
            controller.TempData["BreadcrumbText"] = breadcrumbText;
        }

        public static string GetBreadcrumbText(this Controller controller)
        {
            return Convert.ToString(controller.TempData["BreadcrumbText"]);
        }
    }
}
