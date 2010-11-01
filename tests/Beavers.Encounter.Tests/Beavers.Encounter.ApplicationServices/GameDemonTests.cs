using System;
using System.Threading;
using NUnit.Framework;
using Rhino.Mocks;
using Beavers.Encounter.ApplicationServices;

namespace Tests.Beavers.Encounter.ApplicationServices
{
    [TestFixture]
    public class GameDemonTests
    {
        [Test]
        public void RecalcTest()
        {
            MockRepository moks = new MockRepository();
            IRecalcGameStateService service = moks.DynamicMock<IRecalcGameStateService>();
            
            Expect.Call(() => service.RecalcGameState(DateTime.Now))
                .IgnoreArguments();

            moks.ReplayAll();

            GameDemon demon = new GameDemon(service);
            demon.MinimalRecalcPeriod = 0;
            demon.RecalcGameState(null);

            moks.VerifyAll();
        }

        [Test]
        public void RecalcTwoTimesTest()
        {
            MockRepository moks = new MockRepository();
            IRecalcGameStateService service = moks.DynamicMock<IRecalcGameStateService>();

            Expect.Call(() => service.RecalcGameState(DateTime.Now))
                .Repeat.Times(2)
                .IgnoreArguments();

            moks.ReplayAll();

            GameDemon demon = new GameDemon(service);
            demon.MinimalRecalcPeriod = 0;
            demon.RecalcGameState(null);
            demon.RecalcGameState(null);

            moks.VerifyAll();
        }

        [Test]
        public void RecalcPeriodTest()
        {
            MockRepository moks = new MockRepository();
            IRecalcGameStateService service = moks.DynamicMock<IRecalcGameStateService>();

            Expect.Call(() => service.RecalcGameState(DateTime.Now))
                .Repeat.Times(1)
                .IgnoreArguments();

            moks.ReplayAll();

            GameDemon demon = new GameDemon(service);
            demon.MinimalRecalcPeriod = 1;
            demon.RecalcGameState(null);
            demon.RecalcGameState(null);

            moks.VerifyAll();
        }

        [Test]
        public void CanHandleExceptionTest()
        {
            MockRepository moks = new MockRepository();
            IRecalcGameStateService service = moks.DynamicMock<IRecalcGameStateService>();

            Expect.Call(() => service.RecalcGameState(DateTime.Now))
                .Throw(new Exception("Error"))
                .IgnoreArguments();

            moks.ReplayAll();

            GameDemon demon = new GameDemon(service);
            demon.MinimalRecalcPeriod = 0;
            demon.RecalcGameState(null);

            moks.VerifyAll();
        }

        [Test]
        public void CanLockConcurentThreadsTest()
        {
            MockRepository moks = new MockRepository();
            IRecalcGameStateService service = moks.DynamicMock<IRecalcGameStateService>();

            GameDemon demon = new GameDemon(service);
            demon.MinimalRecalcPeriod = 0;

            Expect.Call(() => service.RecalcGameState(DateTime.Now))
                .Do((Action<DateTime>)( dateTime => demon.RecalcGameState(DateTime.Now)))
                .IgnoreArguments();

            moks.ReplayAll();

            demon.RecalcGameState(null);

            moks.VerifyAll();
        }

        [Test]
        public void CanStartStopTest()
        {
            MockRepository moks = new MockRepository();
            IRecalcGameStateService service = moks.DynamicMock<IRecalcGameStateService>();

            Expect.Call(() => service.RecalcGameState(DateTime.Now))
                .IgnoreArguments();

            moks.ReplayAll();

            GameDemon demon = new GameDemon(service);
            demon.MinimalRecalcPeriod = 0;
            demon.Start();
            Thread.Sleep(1000);
            demon.Stop();

            moks.VerifyAll();
        }
    }
}
