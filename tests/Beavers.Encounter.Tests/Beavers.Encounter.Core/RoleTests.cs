using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.Core;
using NUnit.Framework;
using SharpArch.Core.DomainModel;
using SharpArch.Testing;

namespace Tests.Beavers.Encounter.Core
{
    [TestFixture]
    public class RoleTests
    {
        [Test]
        public void CanCompareUsers()
        {
            Role instance = new Role() { Name = "Admin"};
            EntityIdSetter.SetIdOf(instance, 1);

            Role instanceToCompareTo = new Role() { Name = "Admin" };
            EntityIdSetter.SetIdOf(instanceToCompareTo, 1);

            instance.ShouldEqual(instanceToCompareTo);
        }
    }
}
