using NUnit.Framework;
using Beavers.Encounter.Core;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Beavers.Encounter.Core
{
	[TestFixture]
    public class CodeTests
    {
        [Test]
        public void CanCompareCodes() {
            Code instance = new Code();
			instance.Name = "New Code";

            Code instanceToCompareTo = new Code();
			instanceToCompareTo.Name = "New Code";

			instance.ShouldEqual(instanceToCompareTo);
        }
    }
}
