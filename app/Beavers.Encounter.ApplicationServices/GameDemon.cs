using System;
using System.IO;
using System.Threading;
using System.Web;
using log4net;
using SharpArch.Core;

namespace Beavers.Encounter.ApplicationServices
{
    public sealed class GameDemon : IGameDemon
    {
        private readonly IRecalcGameStateService recalcGameStateService;
        private Timer timer;

        private int doers;
        private DateTime lastUpdate = DateTime.MinValue;

        // Не позволяем производить пересчет состояния чаще 2 секунд
        public int MinimalRecalcPeriod = 2;

        public GameDemon(IRecalcGameStateService recalcGameStateService)
        {
            Check.Require(recalcGameStateService != null, "recalcGameStateService may not be null");

            this.recalcGameStateService = recalcGameStateService;
        }

        public void Start()
        {
            Check.Require(timer == null, "timer must be null");

            // Запускаем пересчет каждые 3 секунды
            timer = new Timer(RecalcGameState, null, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 3));
        }

        public void Stop()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        public void RecalcGameState(object o)
        {
            // TODO: Сделать блокировку доступа к полю LastUpdate
            if ((DateTime.Now - lastUpdate).TotalSeconds < MinimalRecalcPeriod)
            {
                return;
            }

            if (doers > 0)
            {
                LogManager.GetLogger("LogToFile").Warn(String.Format(
                    "Ахтунг!!! Doers = {0}", doers));
                return;
            }

            // TODO: Сделать блокировку доступа к полю Doers
            try
            {
                doers++;

                SetDummyHttpContext();

                recalcGameStateService.RecalcGameState(DateTime.Now);
            }
            catch (Exception e)
            {
                LogManager.GetLogger("LogToFile").Warn(String.Format(
                    "Ахтунг ошибка!!! Doers = {0}, {1}", doers, e));
            }
            finally
            {
                doers--;
                lastUpdate = DateTime.Now;
            }
        }

        private static void SetDummyHttpContext()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("file", "http://local", "queryString"),
                new HttpResponse(new StringWriter()));
        }
    }
}
