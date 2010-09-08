using System;
using System.Linq.Expressions;
using System.Web.Mvc;

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
            return new RedirectToRouteResult(Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression<T>(action));
        }

        public static RedirectToRouteResult RedirectToAction<T>(this T controller, Expression<Action<T>> action) where T : Controller
        {
            return ((Controller)controller).RedirectToAction<T>(action);
        }
    }


}
