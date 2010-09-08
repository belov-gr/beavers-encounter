using System;
using Beavers.Encounter.Core.DataInterfaces;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;
using System.Collections.Generic;
using System.Web.Mvc;
using Beavers.Encounter.Core;
using Beavers.Encounter.Web.Controllers;
 

namespace Tests.Beavers.Encounter.Web.Controllers
{
    [TestFixture]
    public class TipsControllerTests
    {
        [SetUp]
        public void SetUp() {
            ServiceLocatorInitializer.Init();
            controller = new TipsController(
                CreateMockTipRepository(),
                TasksControllerTests.CreateMockTaskRepository(),
                MockRepository.GenerateMock<IUserRepository>());
        }

        [Test]
        public void CanInitTipCreation() {
            ViewResult result = controller.Create(1).AssertViewRendered();
            
            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TipsController.TipFormViewModel));
            (result.ViewData.Model as TipsController.TipFormViewModel).Tip.ShouldNotBeNull();
        }

        [Test]
        public void CanEnsureTipCreationIsValid() {
            Tip tipFromForm = new Tip();
            ViewResult result = controller.Create(tipFromForm).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TipsController.TipFormViewModel));
        }

        [Test]
        public void CanCreateTip() {
            Tip tipFromForm = CreateTransientTip();
            RedirectToRouteResult redirectResult = controller.Create(tipFromForm)
                .AssertActionRedirect().ToAction("Edit");
            controller.Message.ShouldContain("успешно создана");
        }

        [Test]
        public void CanUpdateTip() {
            Tip tipFromForm = CreateTransientTip();
            EntityIdSetter.SetIdOf<int>(tipFromForm, 1);
            RedirectToRouteResult redirectResult = controller.Edit(tipFromForm)
                .AssertActionRedirect().ToAction("Edit");
            controller.Message.ShouldContain("успешно изменена");
        }

        [Test]
        public void CanInitTipEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

			result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TipsController.TipFormViewModel));
            (result.ViewData.Model as TipsController.TipFormViewModel).Tip.Id.ShouldEqual(1);
        }

        [Test]
        public void CanDeleteTip() {
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Edit");

            controller.Message.ShouldContain("успешно удалена");
        }

		#region Create Mock Tip Repository

        public static IRepository<Tip> CreateMockTipRepository() {

            IRepository<Tip> mockedRepository = MockRepository.GenerateMock<IRepository<Tip>>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateTips());
            mockedRepository.Expect(mr => mr.Get(1)).IgnoreArguments().Return(CreateTip());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateTip());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

			IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
			mockedDbContext.Stub(c => c.CommitChanges());
			mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);
            
            return mockedRepository;
        }

        public static Tip CreateTip()
        {
            Tip tip = CreateTransientTip();
            EntityIdSetter.SetIdOf<int>(tip, 1);
            return tip;
        }

        public static List<Tip> CreateTips()
        {
            List<Tip> tips = new List<Tip>();

            // Create a number of domain object instances here and add them to the list

            return tips;
        }
        
        #endregion

        /// <summary>
        /// Creates a valid, transient Tip; typical of something retrieved back from a form submission
        /// </summary>
        public static Tip CreateTransientTip()
        {
            Tip tip = new Tip() {
				Name = "New tip",
				SuspendTime = 30,
				Task = new Task()
            };
            
            return tip;
        }

        private TipsController controller;
    }
}
