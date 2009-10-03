using NUnit.Framework;
using Beavers.Encounter.Core;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Beavers.Encounter.Core
{
	[TestFixture]
    public class BonusTaskTests
    {
        [Test]
        public void CanCompareBonusTasks() {
            BonusTask instance = new BonusTask();
			instance.Name = "New Code";

            BonusTask instanceToCompareTo = new BonusTask();
			instanceToCompareTo.Name = "New Code";

			instance.ShouldEqual(instanceToCompareTo);
        }
    }
}
