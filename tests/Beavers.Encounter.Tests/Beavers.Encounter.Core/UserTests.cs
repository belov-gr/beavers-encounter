using System;
using NUnit.Framework;
using Beavers.Encounter.Core;

namespace Tests.Beavers.Encounter.Core
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void CanCompareUsers()
        {
            User instance = new User();
            instance.Login = "Beaver";

            User instanceToCompareTo = new User();
            instanceToCompareTo.Login = "Beaver";

            instance.ShouldEqual(instanceToCompareTo);
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "ѕользователь не может быть автором игры.")]
        public void CannotAdminostratorOwnGame()
        {
            User user = new User { Role = Role.Administrator };
            user.Game = new Game();
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "ѕользователь не может быть автором игры.")]
        public void CannotGuestOwnGame()
        {
            User user = new User { Role = Role.Guest };
            user.Game = new Game();
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "„лен команды не может быть автором игры.")]
        public void CannotPlayerOwnGame()
        {
            User user = new User { Team = new Team() };
            user.Game = new Game();
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "„лен команды не может быть автором игры.")]
        public void CannotTeamLeaderOwnGame()
        {
            User user = new User { Role = Role.TeamLeader, Team = new Team() };
            user.Game = new Game();
        }

        [Test]
        public void CanPlayerOwnGame()
        {
            User user = new User { Role = Role.Player };
            user.Game = new Game();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CannotAdminostratorSigninTeamTeam()
        {
            User user = new User { Role = Role.Administrator };
            user.Team = new Team();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CannotAuthorSigninTeamTeam()
        {
            User user = new User { Game = new Game() };
            user.Team = new Team();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CannotGuestSigninTeamTeam()
        {
            User user = new User { Role = Role.Guest };
            user.Team = new Team();
        }
    }
}
