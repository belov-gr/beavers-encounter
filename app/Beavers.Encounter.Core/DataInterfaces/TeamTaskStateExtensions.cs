using System.Collections.Generic;
using System.Linq;

namespace Beavers.Encounter.Core.DataInterfaces
{
    public static class TeamTaskStateExtensions
    {
        /// <summary>
        /// Подсчет зачтенных бонусов.
        /// </summary>
        public static int BonusCodesCount(this IList<TeamTaskState> acceptedTasks)
        {
            int count = 0;
            foreach (TeamTaskState taskState in acceptedTasks)
            {
                if (taskState.State == (int)TeamTaskStateFlag.Execute || taskState.State == (int)TeamTaskStateFlag.Success)
                    count += taskState.AcceptedCodes.Count(x => x.Code.IsBonus == 1);
            }
            return count;
        }

        /// <summary>
        /// Возвращает колекцию подсказок задания, которые получила команда.
        /// </summary>
        public static IEnumerable<Tip> Tips(this IList<AcceptedTip> acceptedTips)
        {
            foreach(AcceptedTip tip in acceptedTips)
            {
                yield return tip.Tip;
            }
        }
    }
}
