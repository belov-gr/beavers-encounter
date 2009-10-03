using System;
using Beavers.Encounter.ApplicationServices;
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
    public class UsersControllerTests
    {
        [SetUp]
        public void SetUp() {
            ServiceLocatorInitializer.Init();

            userService = MockRepository.GenerateStub<IUserService>();
            userService.Stub(x => x.HashPassword(Arg<string>.Is.Anything)).Do(new Func<string, string>(s => s + "HASHED"));
            
            controller = new UsersController(CreateMockUserRepository(), userService);
        }

        /// <summary>
        /// Add a couple of objects to the list within CreateUsers and change the 
        /// "ShouldEqual(0)" within this test to the respective number.
        /// </summary>
        [Test]
        public void CanListUsers() {
            ViewResult result = controller.Index().AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as List<User>).Count.ShouldEqual(0);
        }

        [Test]
        public void CanShowUser() {
            ViewResult result = controller.Show(1).AssertViewRendered();

			result.ViewData.ShouldNotBeNull();
			
            (result.ViewData.Model as User).Id.ShouldEqual(1);
        }

        [Test]
        public void CanInitUserCreation() {
            ViewResult result = controller.Create().AssertViewRendered();
            
            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(UsersController.UserFormViewModel));
            (result.ViewData.Model as UsersController.UserFormViewModel).User.ShouldBeNull();
        }

        [Test]
        public void CanEnsureUserCreationIsValid() {
            User userFromForm = new User();
            ViewResult result = controller.Create(userFromForm).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(UsersController.UserFormViewModel));
        }

        [Test]
        public void CanCreateUser() {
            User userFromForm = CreateTransientUser();
            RedirectToRouteResult redirectResult = controller.Create(userFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.Message.ShouldContain("was successfully created");
        }

        [Test]
        public void CanUpdateUser() {
            User userFromForm = CreateTransientUser();
            EntityIdSetter.SetIdOf<int>(userFromForm, 1);
            RedirectToRouteResult redirectResult = controller.Edit(userFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.Message.ShouldContain("was successfully updated");
        }

        [Test]
        public void CanInitUserEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

			result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(UsersController.UserFormViewModel));
            (result.ViewData.Model as UsersController.UserFormViewModel).User.Id.ShouldEqual(1);
        }

        [Test]
        public void CanDeleteUser() {
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Index");

            controller.Message.ShouldContain("was successfully deleted");
        }

		#region Create Mock User Repository

        internal static IUserRepository CreateMockUserRepository()
        {
            IUserRepository mockedRepository = MockRepository.GenerateMock<IUserRepository>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateUsers());
            mockedRepository.Expect(mr => mr.Get(1)).IgnoreArguments().Return(CreateUser());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateUser());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

			IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
			mockedDbContext.Stub(c => c.CommitChanges());
			mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);

            return mockedRepository;
        }

        internal static User CreateUser() {
            User user = CreateTransientUser();
            EntityIdSetter.SetIdOf<int>(user, 1);
            return user;
        }

        internal static List<User> CreateUsers()
        {
            List<User> users = new List<User>();

            // Create a number of domain object instances here and add them to the list

            return users;
        }
        
        #endregion

        /// <summary>
        /// Creates a valid, transient User; typical of something retrieved back from a form submission
        /// </summary>
        internal static User CreateTransientUser()
        {
            User user = new User() {
				Login = "Beaver",
				Password = "Encounter",
				Nick = "Beaver",
				Phone = "123456789",
				Icq = "123456789",
				Team = null
            };
            
            return user;
        }

        private UsersController controller;
        private IUserService userService;
    }
}
