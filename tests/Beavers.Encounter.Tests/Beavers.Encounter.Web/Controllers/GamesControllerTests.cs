using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
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
using Beavers.Encounter.ApplicationServices;


namespace Tests.Beavers.Encounter.Web.Controllers
{
    [TestFixture]
    public class GamesControllerTests
    {
        private User user;
        private ControllerTestContext testContext;

        [SetUp]
        public void SetUp() 
        {
            ServiceLocatorInitializer.Init();

            controller = new GamesController(
                CreateMockGameRepository(), 
                MockRepository.GenerateMock<IUserRepository>(),
                MockRepository.GenerateMock<IGameService>());
            testContext = new ControllerTestContext(controller);

            user = new User {Game = new Game()};
        }

        /// <summary>
        /// Add a couple of objects to the list within CreateGames and change the 
        /// "ShouldEqual(0)" within this test to the respective number.
        /// </summary>
        [Test]
        public void CanListGames() {
            ViewResult result = controller.Index().AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            (result.ViewData.Model as List<Game>).Count.ShouldEqual(0);
        }

        [Test]
        public void CanShowGame() {
            ViewResult result = controller.Show(1).AssertViewRendered();

			result.ViewData.ShouldNotBeNull();
			
            (result.ViewData.Model as Game).Id.ShouldEqual(1);
        }

        [Test]
        public void CanInitGameCreation() {
            ViewResult result = controller.Create().AssertViewRendered();
            
            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(GamesController.GameFormViewModel));
            (result.ViewData.Model as GamesController.GameFormViewModel).Game.ShouldBeNull();
        }

        [Test]
        public void CanEnsureGameCreationIsValid() {
            Game gameFromForm = new Game();
            ViewResult result = controller.Create(gameFromForm).AssertViewRendered();

            result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(GamesController.GameFormViewModel));
        }

        [Test]
        public void CanCreateGame() {
            testContext.TestContext.Context.User = user;

            Game gameFromForm = CreateTransientGame();
            RedirectToRouteResult redirectResult = controller.Create(gameFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.Message.ShouldContain("успешно создана");
        }

        [Test]
        public void CanUpdateGame() {
            Game gameFromForm = CreateTransientGame();
            EntityIdSetter.SetIdOf<int>(gameFromForm, 1);
            RedirectToRouteResult redirectResult = controller.Edit(gameFromForm)
                .AssertActionRedirect().ToAction("Index");
            controller.Message.ShouldContain("успешно изменена");
        }

        [Test]
        public void CanInitGameEdit() {
            ViewResult result = controller.Edit(1).AssertViewRendered();

			result.ViewData.Model.ShouldNotBeNull();
            result.ViewData.Model.ShouldBeOfType(typeof(GamesController.GameFormViewModel));
            (result.ViewData.Model as GamesController.GameFormViewModel).Game.Id.ShouldEqual(1);
        }

        [Test]
        public void CanDeleteGame() {
            testContext.TestContext.Context.User = user;
            RedirectToRouteResult redirectResult = controller.Delete(1)
                .AssertActionRedirect().ToAction("Index");

            controller.Message.ShouldContain("успешно удалена");
        }

		#region Create Mock Game Repository

        public static IRepository<Game> CreateMockGameRepository() {

            IRepository<Game> mockedRepository = MockRepository.GenerateMock<IRepository<Game>>();
            mockedRepository.Expect(mr => mr.GetAll()).Return(CreateGames());
            mockedRepository.Expect(mr => mr.Get(1)).IgnoreArguments().Return(CreateGame());
            mockedRepository.Expect(mr => mr.SaveOrUpdate(null)).IgnoreArguments().Return(CreateGame());
            mockedRepository.Expect(mr => mr.Delete(null)).IgnoreArguments();

			IDbContext mockedDbContext = MockRepository.GenerateStub<IDbContext>();
			mockedDbContext.Stub(c => c.CommitChanges());
			mockedRepository.Stub(mr => mr.DbContext).Return(mockedDbContext);
            
            return mockedRepository;
        }

        private static Game CreateGame()
        {
            Game game = CreateTransientGame();
            EntityIdSetter.SetIdOf<int>(game, 1);
            return game;
        }

        private static List<Game> CreateGames()
        {
            List<Game> games = new List<Game>();

            // Create a number of domain object instances here and add them to the list
            //games.Add(CreateGame());

            return games;
        }
        
        #endregion

        /// <summary>
        /// Creates a valid, transient Game; typical of something retrieved back from a form submission
        /// </summary>
        private static Game CreateTransientGame()
        {
            Game game = new Game() {
				Name = "New gme",
				GameDate = DateTime.Parse("01.01.1975 0:00:00"),
				Description = "Game Description",
				TotalTime = 9*60,
				TimePerTask = 90,
                TimePerTip = 30
            };
            
            return game;
        }

        private GamesController controller;
    }
}
