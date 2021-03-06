﻿using MvcContrib.FluentHtml.Behaviors;
using MvcContrib.FluentHtml.Elements;

namespace Beavers.Encounter.Web.HtmlHelpers
{
    public class LowercaseFirstCharacterOfNameBehaviour : IBehavior<IMemberElement>
    {
        public void Execute(IMemberElement element)
        {
            string name;
            if (element.Builder.Attributes.TryGetValue("name", out name) && !string.IsNullOrEmpty(name))
            {
                name = name[0].ToString().ToLower() + name.Substring(1);
                element.Builder.MergeAttribute("name", name, true);
            }
        }
    }
}