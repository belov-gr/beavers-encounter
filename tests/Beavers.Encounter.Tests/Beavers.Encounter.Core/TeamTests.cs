using NUnit.Framework;
using Beavers.Encounter.Core;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Beavers.Encounter.Core
{
	[TestFixture]
    public class TeamTests
    {
        [Test]
        public void CanCompareTeams() {
            Team instance = new Team();
			instance.Name = "Beavers";

            Team instanceToCompareTo = new Team();
			instanceToCompareTo.Name = "Beavers";

			instance.ShouldEqual(instanceToCompareTo);
        }
    }
}
