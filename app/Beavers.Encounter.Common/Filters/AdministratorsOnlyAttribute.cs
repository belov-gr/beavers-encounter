using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Beavers.Encounter.Common.Filters
{
    public class AdministratorsOnlyAttribute : AuthorizeAttribute
    {
        public AdministratorsOnlyAttribute()
        {
            Roles = "Administrator";
            Order = 1; //Must come AFTER AuthenticateAttribute
        }
    }
}
