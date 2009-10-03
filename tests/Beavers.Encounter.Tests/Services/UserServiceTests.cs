using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Beavers.Encounter.ApplicationServices;
using Beavers.Encounter.Core;
using NUnit.Framework;
using Rhino.Mocks;
using SharpArch.Core.PersistenceSupport;
using Tests.Beavers.Encounter.Web.Controllers;

namespace Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        IUserService userService;
        IRepository<User> userRepository;
        IFormsAuthentication formsAuthentication;

        [SetUp]
        public void SetUp()
        {
            userRepository = UsersControllerTests.CreateMockUserRepository();
            formsAuthentication = MockRepository.GenerateStub<IFormsAuthentication>();
            userService = new UserService(userRepository, formsAuthentication);
        }

        [Test]
        public void CreateNewCustomer_ShouldReturnANewCustomerAddedToTheRepository()
        {
            userRepository.Expect(ur => ur.SaveOrUpdate(Arg<User>.Is.Anything));
            //!!!userRepository.Expect(ur => ur.SubmitChanges());

            User user = userService.CreateNewUser();

            Assert.IsNotNull(user, "returned user is null");
            //!!!Assert.IsTrue(user.IsCustomer, "returned user is not a customer");
        }

        [Test]
        public void HashPassword_should_delegate_to_formsAuthentication()
        {
            formsAuthentication.Expect(x => x.HashPasswordForStoringInConfigFile("foo")).Return("bar");
            userService.HashPassword("foo").ShouldEqual("bar");
        }

        [Test]
        public void RemoveAuthenticationCookie_should_delegate_to_formsAuthentication()
        {
            userService.RemoveAuthenticationCookie();
            formsAuthentication.AssertWasCalled(x => x.SignOut());
        }

        [Test]
        public void SetAuthenticationCookie_should_delegate_to_formsAuthentication()
        {
            userService.SetAuthenticationCookie("foo@foo.com");
            formsAuthentication.AssertWasCalled(x => x.SetAuthCookie("foo@foo.com", true));
        }

        //[Test]
        //!!!public void Authenticate_should_check_that_user_and_matching_password_exist_in_repository()
        //{
        //    formsAuthentication.Stub(x => x.HashPasswordForStoringInConfigFile("foo")).Return("bar");

        //    var user = new User
        //    {
        //        Login = "foo@foo.com",
        //        Password = "bar",
        //        //!!!IsEnabled = true
        //    };
        //    userRepository.Stub(r => r.GetAll()).Return(new[] { user }.AsEnumerable());

        //    var isAuthenticated = userService.Authenticate("foo@foo.com", "foo");

        //    Assert.That(isAuthenticated, Is.True);
        //}
    }
}
