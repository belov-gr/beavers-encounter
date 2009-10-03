using NUnit.Framework;
using Beavers.Encounter.Core;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Beavers.Encounter.Core
{
	[TestFixture]
    public class GameTests
    {
        [Test]
        public void CanCompareGames() {
            Game instance = new Game();
			instance.Name = "New gme";

            Game instanceToCompareTo = new Game();
			instanceToCompareTo.Name = "New gme";

			instance.ShouldEqual(instanceToCompareTo);
        }
    }
}
