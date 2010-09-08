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
using Tests.TestHelpers;


namespace Tests.Beavers.Encounter.Web.Controllers
{
    [TestFixture]
    public class TeamsControllerTests
    {
        private User user;
        private ControllerTestContext testContext;

        [SetUp]
        public void SetUp() 
        {
            ServiceLocatorInitializer.Init();
            controller = new TeamsController(
                CreateMockTeamRepository(),
                MockRepository.GenerateMock<IUserRepository>(),
                GamesControllerTests.CreateMockGameRepository());

            testContext = new ControllerTestContext(controller);
            user = new User { Team = new Team()};
        }

        /// <summary>
        /// Add a couple of objects to the list within CreateTeams and change the 
        /// "ShouldEqual(0)" within this test to the respective number.
        /// </summary>
        [Test]
        public void CanListTeams() {
            ViewResult result = controller.Index().AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as List<Team>).Count.ShouldEqual(0);
        }

        [Test]
        public void CanShowTeam() 
        {
            ViewResult result = controller.Show(1).AssertViewRendered();

			result.ViewData.ShouldNotBeNull();
			
            (result.ViewData.Model as Team).Id.ShouldEqual(1);
        }

        [Test]
        public void CanInitTeamCreation() {
            ViewResult result = controller.Create().AssertViewRendered();
            
            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TeamsController.TeamFormViewModel));
            (result.ViewData.Model as TeamsController.TeamFormViewModel).Team.ShouldBeNull();
        }

        [Test]
        public void CanEnsureTeamCreationIsValid() {
            Team teamFromForm = new Team();
            ViewResult result = controller.Create(teamFromForm).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TeamsController.TeamFormViewModel));
        }

        [Test]
        public void CanCreateTeam() {
            testContext.TestContext.Context.User = new User { Login = "Test", Role = new Role(Role.AuthorId), Game = new Game() };

            Team teamFromForm = CreateTransientTeam();
            RedirectToRouteResult redirectResult = controller.Create(teamFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.Message.ShouldContain("успешно создана");
        }

        [Test]
        public void CanUpdateTeam() {
            Team teamFromForm = CreateTransientTeam();
            EntityIdSetter.SetIdOf<int>(teamFromForm, 1);
            RedirectToRouteResult redirectResult = controller.Edit(teamFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.Message.ShouldContain("успешно изменена");
        }

        [Test]
        public void CanInitTeamEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

			result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(TeamsController.TeamFormViewModel));
            (result.ViewData.Model as TeamsController.TeamFormViewModel).Team.Id.ShouldEqual(1);
        }

        [Test]
        public void CanDeleteTeam() {
            testContext.TestContext.Context.User = user;
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Index");

            controller.Message.ShouldContain("успешно удалена");
        }

		#region Create Mock Team Repository

        private IRepository<Team> CreateMockTeamRepository() {

            IRepository<Team> mockedRepository = MockRepository.GenerateMock<IRepository<Team>>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateTeams());
            mockedRepository.Expect(mr => mr.Get(1)).IgnoreArguments().Return(CreateTeam());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateTeam());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

			IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
			mockedDbContext.Stub(c => c.CommitChanges());
			mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);
            
            return mockedRepository;
        }

        private Team CreateTeam() {
            Team team = CreateTransientTeam();
            EntityIdSetter.SetIdOf<int>(team, 1);
            return team;
        }

        private List<Team> CreateTeams() {
            List<Team> teams = new List<Team>();

            // Create a number of domain object instances here and add them to the list

            return teams;
        }

        #endregion

        /// <summary>
        /// Creates a valid, transient Team; typical of something retrieved back from a form submission
        /// </summary>
        private Team CreateTransientTeam() {
            Team team = new Team() {
				Name = "Beavers"
            };
            
            return team;
        }

        private TeamsController controller;
    }
}
