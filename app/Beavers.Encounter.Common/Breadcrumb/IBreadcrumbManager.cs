namespace Beavers.Encounter.Common
{
    public interface IBreadcrumbManager
    {
        Breadcrumb[] PushBreadcrumb(string link, string text, int level);
    }
}
