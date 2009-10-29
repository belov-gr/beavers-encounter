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
using Tests.Repositories;
using Tests.TestHelpers;


namespace Tests.Beavers.Encounter.Web.Controllers
{
    [TestFixture]
    public class TasksControllerTests
    {
        private User user;
        private ControllerTestContext testContext;

        [SetUp]
        public void SetUp() {
            ServiceLocatorInitializer.Init();
            
            controller = new TasksController(
                CreateMockTaskRepository(),
                MockRepository.GenerateMock<IUserRepository>(), 
                TipsControllerTests.CreateMockTipRepository(), 
                CodesControllerTests.CreateMockCodeRepository());
            
            testContext = new ControllerTestContext(controller);
            user = new User { Game = new Game() };
        }

        /// <summary>
        /// Add a couple of objects to the list within CreateTasks and change the 
        /// "ShouldEqual(0)" within this test to the respective number.
        /// </summary>
        [Test]
        public void CanListTasks() {
            testContext.TestContext.Context.User = user;
            ViewResult result = controller.Index().AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as List<Task>).Count.ShouldEqual(0);
        }

        [Test]
        public void CanShowTask() {
            ViewResult result = controller.Show(1).AssertViewRendered();

			result.ViewData.ShouldNotBeNull();
			
            (result.ViewData.Model as Task).Id.ShouldEqual(1);
        }

        [Test]
        public void CanInitTaskCreation() {
            ViewResult result = controller.Create().AssertViewRendered();
            
            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TasksController.TaskFormViewModel));
            (result.ViewData.Model as TasksController.TaskFormViewModel).Task.ShouldBeNull();
        }

        [Test]
        public void CanEnsureTaskCreationIsValid() {
            Task taskFromForm = new Task();
            ViewResult result = controller.Create(taskFromForm).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TasksController.TaskFormViewModel));
        }

        [Test]
        public void CanCreateTask() {
            testContext.TestContext.Context.User = user;
            Task taskFromForm = CreateTransientTask();
            RedirectToRouteResult redirectResult = controller.Create(taskFromForm)
                .AssertActionRedirect().ToAction("Edit");
            controller.Message.ShouldContain("was successfully created");
        }

        [Test]
        public void CanUpdateTask() {
            Task taskFromForm = CreateTransientTask();
            EntityIdSetter.SetIdOf<int>(taskFromForm, 1);
            RedirectToRouteResult redirectResult = controller.Edit(taskFromForm)
                .AssertActionRedirect().ToAction("Edit");
            controller.Message.ShouldContain("was successfully updated");
        }

        [Test]
        public void CanInitTaskEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

			result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TasksController.TaskFormViewModel));
            (result.ViewData.Model as TasksController.TaskFormViewModel).Task.Id.ShouldEqual(1);
        }

        [Test]
        public void CanDeleteTask() {
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Edit");

            controller.Message.ShouldContain("was successfully deleted");
        }

		#region Create Mock Task Repository

        public static ITaskRepository CreateMockTaskRepository()
        {
            ITaskRepository mockedRepository = MockRepository.GenerateMock<ITaskRepository>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateTasks());
            mockedRepository.Expect(mr => mr.GetByGame(null)).IgnoreArguments().Return(CreateTasks());
            mockedRepository.Expect(mr => mr.Get(1)).IgnoreArguments().Return(CreateTask());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateTask());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

			IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
			mockedDbContext.Stub(c => c.CommitChanges());
			mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);
            
            return mockedRepository;
        }

        public static Task CreateTask()
        {
            Task task = CreateTransientTask();
            EntityIdSetter.SetIdOf<int>(task, 1);
            return task;
        }

        public static List<Task> CreateTasks()
        {
            List<Task> tasks = new List<Task>();

            // Create a number of domain object instances here and add them to the list

            return tasks;
        }
        
        #endregion

        /// <summary>
        /// Creates a valid, transient Task; typical of something retrieved back from a form submission
        /// </summary>
        public static Task CreateTransientTask()
        {
            Task task = new Task() {
				Name = "New task",
                Game = new Game() { Name = "New game" }
            };

            EntityIdSetter.SetIdOf<int>(task.Game, 1);
            return task;
        }

        private TasksController controller;
    }
}
