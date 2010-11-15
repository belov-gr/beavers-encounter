using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using SharpArch.Core;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.NHibernate;

using Beavers.Encounter.Common;
using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Common.MvcContrib;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;

namespace Beavers.Encounter.Web.Controllers
{
    [AdministratorsOnly]
    public class AdminUsersController : BaseController
    {
        private readonly IRepository<Team> teamRepository;
        private readonly IRepository<Game> gameRepository;
        private readonly IMembershipService membershipService;

        public AdminUsersController(IUserRepository userRepository, 
            IRepository<Team> teamRepository, IRepository<Game> gameRepository)
            : this(userRepository, new AccountMembershipService(), teamRepository, gameRepository)
        {
        }

        public AdminUsersController(IUserRepository userRepository, IMembershipService membershipService,
            IRepository<Team> teamRepository, IRepository<Game> gameRepository)
            : base(userRepository)
        {
            Check.Require(teamRepository != null, "teamRepository may not be null");
            Check.Require(gameRepository != null, "gameRepository may not be null");

            this.membershipService = membershipService ?? new AccountMembershipService();
            this.teamRepository = teamRepository;
            this.gameRepository = gameRepository;
        }

        [Breadcrumb("Список пользователей", 3)]
        public ActionResult Index()
        {
            return View(UserRepository.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(User user, string confirmPassword)
        {
            AccountController.ValidateRegistration(user.Login, user.Password, confirmPassword,
                                                   ModelState, membershipService, UserRepository);

            if (ViewData.ModelState.IsValid && user.IsValid())
            {
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "SHA1");
                UserRepository.SaveOrUpdate(user);

                Message = "Пользователь успешно создан.";
                return this.RedirectToAction(c => c.Index());
            }

            return View(user);
        }

        [Breadcrumb("Пользователь \"{0}\"", 4)]
        public ActionResult Edit(int id)
        {
            var viewModel = UserFormViewModel.CreateTipFormViewModel();
            viewModel.User = UserRepository.Get(id);
            viewModel.Teams = teamRepository.GetAll();
            viewModel.Games = gameRepository.GetAll();

            this.SetBreadcrumbText(viewModel.User.Login);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(User user)
        {
            User userToUpdate = UserRepository.Get(user.Id);
            TransferFormValuesTo(userToUpdate, user);

            if (ViewData.ModelState.IsValid && user.IsValid())
            {
                Message = "Изменения успешно сохранены.";
                return this.RedirectToAction(c => c.Index());
            }
            UserRepository.DbContext.RollbackTransaction();

            var viewModel = UserFormViewModel.CreateTipFormViewModel();
            viewModel.User = user;
            viewModel.Teams = teamRepository.GetAll();
            viewModel.Games = gameRepository.GetAll();
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id)
        {
            string resultMessage = "Пользователь успешно удален.";
            var userToDelete = UserRepository.Get(id);

            if (userToDelete != null)
            {
                UserRepository.Delete(userToDelete);
            }
            else
            {
                resultMessage = "Удаляемый пользователь не найден. Возможно, его уже удалили.";
            }

            Message = resultMessage;
            return this.RedirectToAction(c => c.Index());
        }

        private static void TransferFormValuesTo(User userToUpdate, User user)
        {
            userToUpdate.Nick = user.Nick;
            userToUpdate.Icq = user.Icq;
            userToUpdate.Phone = user.Phone;
            userToUpdate.Game = user.Game;
            userToUpdate.Team = user.Team;
            //userToUpdate.IsEnabled = user.IsEnabled;
            //userToUpdate.Role = user.Role;
        }

        public class UserFormViewModel
        {
            private UserFormViewModel() { }

            public static UserFormViewModel CreateTipFormViewModel()
            {
                return new UserFormViewModel();
            }

            public User User { get; internal set; }
            public IList<Team> Teams { get; internal set; }
            public IList<Game> Games { get; internal set; }
        }
    }
}
