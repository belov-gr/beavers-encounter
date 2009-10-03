using NUnit.Framework;
using Beavers.Encounter.Core;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Beavers.Encounter.Core
{
	[TestFixture]
    public class TipTests
    {
        [Test]
        public void CanCompareTips() {
            Tip instance = new Tip();
			instance.Name = "New tip";

            Tip instanceToCompareTo = new Tip();
			instanceToCompareTo.Name = "New tip";

			instance.ShouldEqual(instanceToCompareTo);
        }
    }
}
