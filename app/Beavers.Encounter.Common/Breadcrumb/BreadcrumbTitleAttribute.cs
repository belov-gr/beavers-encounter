using System;

namespace Beavers.Encounter.Common
{
    public class BreadcrumbTitle1Attribute : Attribute
    {
        public BreadcrumbTitle1Attribute(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}
