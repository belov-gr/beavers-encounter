using System.Collections.Generic;
using System.Web.Mvc;
using Beavers.Encounter.Common;
using Beavers.Encounter.Web.HtmlHelpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.Beavers.Encounter.Web.HtmlHelpers
{
    [TestFixture]
    public class BreadcrumbTests
    {
        [Test]
        public void BreadcrumbTest()
        {
            HtmlHelper helper = new HtmlHelper(
                MockRepository.GenerateMock<ViewContext>(),
                MockRepository.GenerateMock<IViewDataContainer>());
            
            List<Breadcrumb> list = new List<Breadcrumb>();
            list.Add(new Breadcrumb { Text = "test1", Link = "link1" });
            list.Add(new Breadcrumb { Text = "test2", Link = "link2" });

            var html = helper.Breadcrumb(list.ToArray());

            Assert.AreEqual(
                "<div class='breadcrumbs'><span class='breadcrumb'><a href='link2'>test2</a></span><span class='breadcrumbs-arrow'> » </span><span class='breadcrumb'><a href='link1'>test1</a></span></div>", 
                html);
        }
    }
}
