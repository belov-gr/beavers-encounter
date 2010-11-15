using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Web.Controllers;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Beavers.Encounter.Web.Controllers.Admin
{
    [TestFixture]
    public class AdminUsersControllerTests
    {
        private AdminUsersController controller;

        [SetUp]
        public void SetUp()
        {
            ServiceLocatorInitializer.Init();

            controller = new AdminUsersController(
                CreateMockUserRepository(),
                MockRepository.GenerateMock<IMembershipService>(),
                MockRepository.GenerateMock<IRepository<Team>>(),
                MockRepository.GenerateMock<IRepository<Game>>());
        }

        [Test]
        public void CanListUsers()
        {
            ViewResult result = controller.Index().AssertViewRendered();
            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as List<User>).Count.ShouldEqual(1);
        }

        [Test]
        public void CanInitCreateUser()
        {
            ViewResult result = controller.Create().AssertViewRendered();
            result.ViewData.Model.ShouldBeNull();
        }

        [Test]
        public void CanEnsureUserCreationIsValid()
        {
            User userFromForm = new User { Login = String.Empty, Password = String.Empty };
            ViewResult result = controller.Create(userFromForm, String.Empty).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(User));
        }

        [Test]
        public void CanCreateUser()
        {
            User userFromForm = CreateTransientUser();
            userFromForm.Login = "Login1";
            controller.Create(userFromForm, "password")
                .AssertActionRedirect().ToAction("Index");
            
            controller.Message.ShouldContain("успешно создан");
        }

        [Test]
        public void CanUpdateUser()
        {
            User userFromForm = CreateTransientUser();
            EntityIdSetter.SetIdOf(userFromForm, 1);
            controller.Edit(userFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.Message.ShouldContain("успешно сохранен");
        }

        [Test]
        public void CanEnsureUserEditionIsValid()
        {
            User userFromForm = new User { Login = String.Empty, Password = String.Empty };
            EntityIdSetter.SetIdOf(userFromForm, 1);
            ViewResult result = controller.Edit(userFromForm).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(AdminUsersController.UserFormViewModel));
        }

        [Test]
        public void CanInitUserEdit()
        {
            ViewResult result = controller.Edit(1).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(AdminUsersController.UserFormViewModel));
            (result.ViewData.Model as AdminUsersController.UserFormViewModel).User.Id.ShouldEqual(1);
        }

        [Test]
        public void CanDeleteUser()
        {
            controller.Delete(1).AssertActionRedirect().ToAction("Index");

            controller.Message.ShouldContain("успешно удален");
        }

        [Test]
        public void CanDeleteNotExistsUser()
        {
            controller.Delete(2).AssertActionRedirect().ToAction("Index");

            controller.Message.ShouldContain("не найден");
        }

        #region Mocks

        public static IUserRepository CreateMockUserRepository()
        {
            IUserRepository mockedRepository = MockRepository.GenerateMock<IUserRepository>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateUsers());
            mockedRepository.Expect(mr => mr.Get(1)).Return(CreateUser());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateUser());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

            IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
            mockedDbContext.Stub(c => c.CommitChanges());
            mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);

            return mockedRepository;
        }

        private static User CreateUser()
        {
            User user = CreateTransientUser();
            EntityIdSetter.SetIdOf(user, 1);
            return user;
        }

        private static List<User> CreateUsers()
        {
            List<User> users = new List<User>();

            // Create a number of domain object instances here and add them to the list
            users.Add(CreateUser());

            return users;
        }

        /// <summary>
        /// Creates a valid, transient Game; typical of something retrieved back from a form submission
        /// </summary>
        private static User CreateTransientUser()
        {
            User user = new User
            {
                Login = "login",
                Password = "password"
            };

            return user;
        }

        #endregion Mocks
    }
}
