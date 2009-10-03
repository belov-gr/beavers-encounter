using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.ApplicationServices;
using NUnit.Framework;

namespace Tests.Services
{
    [TestFixture]
    public class GameServiceTest
    {
        [Test]
        public void CanGetCodesFlat()
        {
            IList<string> list = TaskService.GetCodes(" 14D245 , 14b123 14d789 , ", "14d", "14B");
            
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("245", list[0]);
            Assert.AreEqual("123", list[1]);
            Assert.AreEqual("789", list[2]);
        }
    }
}
