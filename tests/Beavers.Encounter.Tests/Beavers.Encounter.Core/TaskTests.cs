using NUnit.Framework;
using Beavers.Encounter.Core;
using SharpArch.Testing;
using SharpArch.Testing.NUnit;

namespace Tests.Beavers.Encounter.Core
{
	[TestFixture]
    public class TaskTests
    {
        [Test]
        public void CanCompareTasks() {
            Task instance = new Task();
			instance.Name = "New task";

            Task instanceToCompareTo = new Task();
			instanceToCompareTo.Name = "New task";

			instance.ShouldEqual(instanceToCompareTo);
        }
    }
}
