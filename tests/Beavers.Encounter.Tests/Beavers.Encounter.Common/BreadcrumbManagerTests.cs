using Beavers.Encounter.Common;
using NUnit.Framework;

namespace Tests.Beavers.Encounter.Common
{
    [TestFixture]
    public class BreadcrumbManagerTests
    {
        [Test]
        public void CanPushTest()
        {
            var breadcrumbManager = new BreadcrumbManager();

            breadcrumbManager.PushBreadcrumb("/", "Home", 1);
            breadcrumbManager.PushBreadcrumb("/Game/Index", "Games", 2);
            breadcrumbManager.PushBreadcrumb("/Game/Show/1", "Game", 3);
            var breadcrumbs = breadcrumbManager.PushBreadcrumb("/Task/Show/1", "Task", 4);

            Assert.AreEqual(4, breadcrumbs.Length);
        }


        [Test]
        public void CanGoBackTest()
        {
            var breadcrumbManager = new BreadcrumbManager();
            
            breadcrumbManager.PushBreadcrumb("/", "Home", 1);
            breadcrumbManager.PushBreadcrumb("/Game/Index", "Games", 2);
            breadcrumbManager.PushBreadcrumb("/Game/Show/1", "Game", 3);
            breadcrumbManager.PushBreadcrumb("/Task/Show/1", "Task", 4);
            breadcrumbManager.PushBreadcrumb("/Tip/Show/1", "Tip", 3);
            var breadcrumbs = breadcrumbManager.PushBreadcrumb("/Game/Show/1", "Game", 3);
            
            Assert.AreEqual(3, breadcrumbs.Length);
        }

        [Test]
        public void CanGoBranchTest()
        {
            var breadcrumbManager = new BreadcrumbManager();

            breadcrumbManager.PushBreadcrumb("/", "Home", 1);
            breadcrumbManager.PushBreadcrumb("/Game/Index", "Games", 2);
            breadcrumbManager.PushBreadcrumb("/Game/Show/1", "Game", 3);
            breadcrumbManager.PushBreadcrumb("/Task/Show/1", "Task", 4);
            var breadcrumbs = breadcrumbManager.PushBreadcrumb("/Admin/Home/Index", "Admin", 2);
            
            Assert.AreEqual(2, breadcrumbs.Length);
        }

        [Test]
        public void CanPushEmptyTextTest()
        {
            var breadcrumbManager = new BreadcrumbManager();

            var breadcrumbs = breadcrumbManager.PushBreadcrumb("/Admin/Home/Index", null, 2);

            Assert.AreEqual(1, breadcrumbs.Length);
            Assert.AreEqual(breadcrumbs[0].Link, breadcrumbs[0].Text);
        }
    }
}
