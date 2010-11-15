using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Web.Controllers;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.Beavers.Encounter.Web.Controllers.Admin
{
    [TestFixture]
    public class AdminControllerTests
    {
        private AdminController controller;

        [SetUp]
        public void SetUp()
        {
            ServiceLocatorInitializer.Init();

            controller = new AdminController(MockRepository.GenerateMock<IUserRepository>());
        }

        [Test]
        public void CanListTest()
        {
            controller.Index().AssertViewRendered();
        }
    }
}
