using System.Web.Mvc;
using log4net;

namespace Beavers.Encounter.Common.Filters
{
    public class AuthorsOnlyAttribute : AuthorizeAttribute
    {
        public AuthorsOnlyAttribute()
        {
            Roles = "Author";
            Order = 1; //Must come AFTER AuthenticateAttribute
        }
    }
}
