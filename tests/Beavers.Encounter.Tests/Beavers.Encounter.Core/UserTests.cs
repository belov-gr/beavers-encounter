using System;
using NUnit.Framework;
using Beavers.Encounter.Core;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Beavers.Encounter.Core
{
	[TestFixture]
    public class UserTests
    {
        [Test]
        public void CanCompareUsers() {
            User instance = new User();
			instance.Login = "Beaver";

            User instanceToCompareTo = new User();
			instanceToCompareTo.Login = "Beaver";

			instance.ShouldEqual(instanceToCompareTo);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CannotAdminostratorCreateGame()
        {
            User user = new User() {Role = Role.Administrator};
            user.Game = new Game();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CannotAdminostratorCreateTeam()
        {
            User user = new User() { Role = Role.Administrator };
            user.Team = new Team();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CannotGuestCreateGame()
        {
            User user = new User() { Role = Role.Guest };
            user.Game = new Game();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CannotGuestCreateTeam()
        {
            User user = new User() { Role = Role.Guest };
            user.Team = new Team();
        }
    }
}
