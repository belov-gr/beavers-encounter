using Beavers.Encounter.Web.HtmlHelpers;

namespace Beavers.Encounter.Web.Views
{
    public class ViewPage<T> : MvcContrib.FluentHtml.ModelViewPage<T> where T : class
    {
        public ViewPage()
            : base(new LowercaseFirstCharacterOfNameBehaviour())
        {

        }
    }

    public class ViewUserControl<T> : MvcContrib.FluentHtml.ModelViewUserControl<T> where T : class
    {
        public ViewUserControl()
            : base(new LowercaseFirstCharacterOfNameBehaviour())
        {

        }
    }
}
