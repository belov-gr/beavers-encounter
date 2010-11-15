using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Beavers.Encounter.Common;

namespace Beavers.Encounter.Web.HtmlHelpers
{
    public static class HtmlBreadcrumbHelper
    {
        public static string Breadcrumb(this HtmlHelper helper)
        {
            Breadcrumb[] breadcrumb = helper.ViewData["breadcrumbs"] as Breadcrumb[];
            if (breadcrumb != null)
            {
                return Breadcrumb(helper, breadcrumb);
            }
            return String.Empty;
        }

        public static string Breadcrumb(this HtmlHelper helper, Breadcrumb[] breadcrumbs)
        {
            string arrow = "<span class='breadcrumbs-arrow'> » </span>";

            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='breadcrumbs'>");
            foreach (var breadcrumb in breadcrumbs.Reverse())
            {
                sb.Append(arrow)
                    .Append("<span class='breadcrumb'><a href='")
                    .Append(breadcrumb.Link)
                    .Append("'>")
                    .Append(breadcrumb.Text)
                    .Append("</a></span>");
            }
            sb.Append("</div>");
            sb.Replace("class='breadcrumbs'>" + arrow, "class='breadcrumbs'>");
            return sb.ToString();
        }
    }
}
