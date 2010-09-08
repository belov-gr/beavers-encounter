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
    public class CodesControllerTests
    {
        [SetUp]
        public void SetUp() {
            ServiceLocatorInitializer.Init();
            controller = new CodesController(
                CreateMockCodeRepository(),
                TasksControllerTests.CreateMockTaskRepository(),
                MockRepository.GenerateMock<IUserRepository>());
        }

        [Test]
        public void CanInitCodeCreation() {
            ViewResult result = controller.Create(1).AssertViewRendered();
            
            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(CodesController.CodeFormViewModel));
            (result.ViewData.Model as CodesController.CodeFormViewModel).Code.ShouldNotBeNull();
        }

        [Test]
        public void CanEnsureCodeCreationIsValid() {
            Code codeFromForm = new Code();
            ViewResult result = controller.Create(codeFromForm).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(CodesController.CodeFormViewModel));
        }

        [Test]
        public void CanCreateCode() {
            Code codeFromForm = CreateTransientCode();
            RedirectToRouteResult redirectResult = controller.Create(codeFromForm)
                .AssertActionRedirect().ToAction("Edit");
            controller.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
                .ShouldContain("успешно создан");
        }

        [Test]
        public void CanUpdateCode() {
            Code codeFromForm = CreateTransientCode();
            EntityIdSetter.SetIdOf<int>(codeFromForm, 1);
            RedirectToRouteResult redirectResult = controller.Edit(codeFromForm)
                .AssertActionRedirect().ToAction("Edit");
            controller.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
                .ShouldContain("успешно изменен");
        }

        [Test]
        public void CanInitCodeEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

			result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(CodesController.CodeFormViewModel));
            (result.ViewData.Model as CodesController.CodeFormViewModel).Code.Id.ShouldEqual(1);
        }

        [Test]
        public void CanDeleteCode() {
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Edit");
            
            controller.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
                .ShouldContain("успешно удален");
        }

		#region Create Mock Code Repository

        public static IRepository<Code> CreateMockCodeRepository() {

            IRepository<Code> mockedRepository = MockRepository.GenerateMock<IRepository<Code>>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateCodes());
            mockedRepository.Expect(mr => mr.Get(1)).IgnoreArguments().Return(CreateCode());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateCode());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

			IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
			mockedDbContext.Stub(c => c.CommitChanges());
			mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);
            
            return mockedRepository;
        }

        private static Code CreateCode() {
            Code code = CreateTransientCode();
            EntityIdSetter.SetIdOf<int>(code, 1);
            return code;
        }

        private static List<Code> CreateCodes() {
            List<Code> codes = new List<Code>();

            // Create a number of domain object instances here and add them to the list

            return codes;
        }
        
        #endregion

        /// <summary>
        /// Creates a valid, transient Code; typical of something retrieved back from a form submission
        /// </summary>
        private static Code CreateTransientCode() {
            Code code = new Code() {
				Name = "New Code",
				Danger = "1",
				Task = new Task()
            };
            
            return code;
        }

        private CodesController controller;
    }
}
