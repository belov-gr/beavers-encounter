using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Microsoft.Web.Mvc.Internal;

namespace Beavers.Encounter.Common.MvcContrib
{
    public static class ControllerExtensions
    {
        // Methods
        public static bool IsController(Type type)
        {
            return ((((type != null) && type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)) && !type.IsAbstract) && typeof(IController).IsAssignableFrom(type));
        }

        public static RedirectToRouteResult RedirectToAction<T>(this Controller controller, Expression<Action<T>> action) where T : Controller
        {
            return new RedirectToRouteResult(ExpressionHelper.GetRouteValuesFromExpression<T>(action));
        }

        public static RedirectToRouteResult RedirectToAction<T>(this T controller, Expression<Action<T>> action) where T : Controller
        {
            return ((Controller)controller).RedirectToAction<T>(action);
        }
    }


}
