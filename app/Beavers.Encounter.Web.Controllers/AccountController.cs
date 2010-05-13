using System;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using Beavers.Encounter.ApplicationServices;
using Microsoft.Practices.ServiceLocation;
using Beavers.Encounter.Core;
using SharpArch.Core.PersistenceSupport;
using Beavers.Encounter.Core.DataInterfaces;
using SharpArch.Web.NHibernate;

namespace Beavers.Encounter.Web.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {
        // For generating random numbers.
        private Random random = new Random();

        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.
        public AccountController()
            : this(null, null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationWrapper();
            MembershipService = service ?? new AccountMembershipService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IMembershipService MembershipService
        {
            get;
            private set;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string returnUrl)
        {

            if (!ValidateLogOn(userName, password))
            {
                return View();
            }

            FormsAuth.SetAuthCookie(userName, rememberMe);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOff()
        {

            FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            //Session["CaptchaImageText"] = GenerateRandomCode();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Transaction]
        public ActionResult Register(string userName, /*string email, */string password, string confirmPassword/*, string antiSpamCode*/)
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            if (ValidateRegistration(userName, /*email, */password, confirmPassword))
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(userName, password/*, email*/);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuth.SetAuthCookie(userName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View();
        }

        [Authorize]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            try
            {
                if (MembershipService.ChangePassword(User.Identity.Name, currentPassword, newPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("_FORM", "Текущий пароль неверен или новый пароль некорректен.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("_FORM", "Текущий пароль неверен или новый пароль некорректен.");
                return View();
            }
        }

        public ActionResult ChangePasswordSuccess()
        {

            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        #region Validation Methods

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", "Вы должны указать свой текущий пароль.");
            }

            if (newPassword == null || newPassword.Length < MembershipService.MinPasswordLength)
            {
                ModelState.AddModelError("newPassword",
                    String.Format(CultureInfo.CurrentCulture,
                         "Новый пароль должен содержать не менее {0} символов.",
                         MembershipService.MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "Новый пароль не совпадает с подтверждением пароля.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "Нужно указать имя пользователя.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "Нужно указать пароль.");
            }
            if (!MembershipService.ValidateUser(userName, password))
            {
                ModelState.AddModelError("_FORM", "Указанные имя пользователя и/или пароль неверны.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateRegistration(string userName, /*string email, */string password, string confirmPassword)
        {
            IUserRepository ur = ServiceLocator.Current.GetInstance<IUserRepository>();
            if (ur.GetAll().Any(u => u.Login.ToUpper() == userName.ToUpper()))
            {
                ModelState.AddModelError("username", "Пользователь с таким именем уже присутствует.");
            }

            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "Нужно указать имя пользователя.");
            }

            if (userName.Length < 2 || userName.Length > 25)
            {
                ModelState.AddModelError("username", "Имя должно быть не короче 2 и не длиннее 25 символов.");
            }

            if (userName.StartsWith(" ") || userName.EndsWith(" "))
            {
                ModelState.AddModelError("username", "Имя не может быть пустым, а также начинаться или заканчиваться пробелами.");
            }

            /*if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "You must specify an email address.");
            }*/
            
            if (String.IsNullOrEmpty(password) || password.Length < MembershipService.MinPasswordLength)
            {
                ModelState.AddModelError("password",
                    String.Format(CultureInfo.CurrentCulture,
                         "Новый пароль должен содержать не менее {0} символов.",
                         MembershipService.MinPasswordLength));
            }
            if (!String.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "Новый пароль не совпадает с подтверждением пароля.");
            }
            return ModelState.IsValid;
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Пользователь с таким именем уже существует. Пожалуйста, введите другое имя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Некорректный пароль. Пожалуйста, введите правильный пароль.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Имя пользователя некорректно. Пожалуйста, введите правильное имя и попробуйте еще раз.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        //
        // Returns a string of six random digits.
        //
        private string GenerateRandomCode()
        {
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, this.random.Next(10).ToString());
            return s;
        }
        #endregion
    }

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password/*, string email*/);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private IRepository<User> _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(IRepository<User> provider)
        {
            _provider = provider ?? ServiceLocator.Current.GetInstance<IUserRepository>();
        }

        public int MinPasswordLength
        {
            get
            {
                return 5;
                //return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            User user = ((IUserRepository)_provider).GetByLogin(userName);
            if (user == null)
            {
                return false;
            }
            
            if (user.Password == FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1"))
            {
                return true;
            }
            return false;
        }

        public MembershipCreateStatus CreateUser(string userName, string password/*, string email*/)
        {
            User user = new User();
            user.Login = userName;
            user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            user.Nick = userName;
            user.Role = Role.Player;
            _provider.SaveOrUpdate(user);
            return MembershipCreateStatus.Success;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            User user = ((IUserRepository)_provider).GetByLogin(userName);
            if (user == null)
            {
                return false;
            }
            if (user.Password == FormsAuthentication.HashPasswordForStoringInConfigFile(oldPassword, "SHA1"))
            {
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "SHA1");
                _provider.SaveOrUpdate(user);
                return true;
            }
            return false;
        }
    }
}
