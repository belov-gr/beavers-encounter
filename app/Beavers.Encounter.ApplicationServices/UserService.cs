using System;
using System.Web;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.ApplicationServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepository;
        private readonly IFormsAuthentication formsAuth;

        public UserService(IRepository<User> userRepository, IFormsAuthentication formsAuth)
        {
            this.userRepository = userRepository;
            this.formsAuth = formsAuth;
        }

        public User CreateNewUser()
        {
            var user = new User
            {
                Login = Guid.NewGuid().ToString(),
                Password = ""/*,
                RoleId = Role.CustomerId*/
            };

            userRepository.SaveOrUpdate(user);
            return user;
        }

        public virtual User CurrentUser
        {
            get
            {
                var user = HttpContext.Current.User as User;
                if (user == null) throw new ApplicationException("HttpContext.User is not a Suteki.Shop.User");
                return user;
            }
        }

        public virtual void SetAuthenticationCookie(string email)
        {
            formsAuth.SetAuthCookie(email, true);
        }

        public virtual void SetContextUserTo(User user)
        {
            System.Threading.Thread.CurrentPrincipal = HttpContext.Current.User = user;
        }

        public virtual void RemoveAuthenticationCookie()
        {
            formsAuth.SignOut();
        }

        public string HashPassword(string password)
        {
            return formsAuth.HashPasswordForStoringInConfigFile(password);
        }

        public bool Authenticate(string login, string password)
        {
            return userRepository.GetAll().ContainsUser(login, HashPassword(password));
        }
    }
}
