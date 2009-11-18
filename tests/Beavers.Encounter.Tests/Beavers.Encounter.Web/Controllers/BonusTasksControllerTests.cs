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
    public class BonusTasksControllerTests
    {
        [SetUp]
        public void SetUp() {
            ServiceLocatorInitializer.Init();
            controller = new BonusTasksController(CreateMockBonusTaskRepository(), MockRepository.GenerateMock<IUserRepository>());
        }

        [Test]
        public void CanShowBonusTask() {
            ViewResult result = controller.Show(1).AssertViewRendered();

			result.ViewData.ShouldNotBeNull();
			
            (result.ViewData.Model as BonusTask).Id.ShouldEqual(1);
        }

        [Test]
        public void CanInitBonusTaskCreation() {
            ViewResult result = controller.Create().AssertViewRendered();
            
            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(BonusTasksController.BonusTaskFormViewModel));
            (result.ViewData.Model as BonusTasksController.BonusTaskFormViewModel).BonusTask.ShouldBeNull();
        }

        [Test]
        public void CanEnsureBonusTaskCreationIsValid() {
            BonusTask bonusTaskFromForm = new BonusTask();
            ViewResult result = controller.Create(bonusTaskFromForm).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(BonusTasksController.BonusTaskFormViewModel));
        }

        [Test]
        public void CanCreateBonusTask() {
            BonusTask bonusTaskFromForm = CreateTransientBonusTask();
            RedirectToRouteResult redirectResult = controller.Create(bonusTaskFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
				.ShouldContain("was successfully created");
        }

        [Test]
        public void CanUpdateBonusTask() {
            BonusTask bonusTaskFromForm = CreateTransientBonusTask();
            EntityIdSetter.SetIdOf<int>(bonusTaskFromForm, 1);
            RedirectToRouteResult redirectResult = controller.Edit(bonusTaskFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
				.ShouldContain("was successfully updated");
        }

        [Test]
        public void CanInitBonusTaskEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

			result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(BonusTasksController.BonusTaskFormViewModel));
            (result.ViewData.Model as BonusTasksController.BonusTaskFormViewModel).BonusTask.Id.ShouldEqual(1);
        }

        [Test]
        public void CanDeleteBonusTask() {
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Index");
            
            controller.TempData[ControllerEnums.GlobalViewDataProperty.PageMessage.ToString()].ToString()
				.ShouldContain("was successfully deleted");
        }

		#region Create Mock BonusTask Repository

        private IRepository<BonusTask> CreateMockBonusTaskRepository() {

            IRepository<BonusTask> mockedRepository = MockRepository.GenerateMock<IRepository<BonusTask>>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateBonusTasks());
            mockedRepository.Expect(mr => mr.Get(1)).IgnoreArguments().Return(CreateBonusTask());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateBonusTask());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

			IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
			mockedDbContext.Stub(c => c.CommitChanges());
			mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);
            
            return mockedRepository;
        }

        private BonusTask CreateBonusTask() {
            BonusTask bonusTask = CreateTransientBonusTask();
            EntityIdSetter.SetIdOf<int>(bonusTask, 1);
            return bonusTask;
        }

        private List<BonusTask> CreateBonusTasks() {
            List<BonusTask> bonusTasks = new List<BonusTask>();

            // Create a number of domain object instances here and add them to the list

            return bonusTasks;
        }
        
        #endregion

        /// <summary>
        /// Creates a valid, transient BonusTask; typical of something retrieved back from a form submission
        /// </summary>
        private BonusTask CreateTransientBonusTask() {
            BonusTask bonusTask = new BonusTask()
            {
                Name = "New Code",
                TaskText = "Task description",
                StartTime = DateTime.Parse("01.01.75 0:00:00"),
                FinishTime = DateTime.Parse("01.01.75 0:00:00"),
                IsIndividual = false,
                Game = new Game() { Name = "New game" }
            };
            
            return bonusTask;
        }

        private BonusTasksController controller;
    }
}
