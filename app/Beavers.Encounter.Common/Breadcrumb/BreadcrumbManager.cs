using System;
using System.Collections.Generic;

namespace Beavers.Encounter.Common
{
    public class BreadcrumbManager : IBreadcrumbManager
    {
        private readonly Stack<Breadcrumb> stack;

        public BreadcrumbManager()
        {
            stack = new Stack<Breadcrumb>();
        }

        public Breadcrumb[] PushBreadcrumb(string link, string text, int level)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = link;
            }

            var bc = new Breadcrumb { Link = link, Text = text, Level = level };

            while (stack.Count > 0 && stack.Peek().Level >= level)
            {
                stack.Pop();
            }

            stack.Push(bc);
            return stack.ToArray();
        }
    }
}
