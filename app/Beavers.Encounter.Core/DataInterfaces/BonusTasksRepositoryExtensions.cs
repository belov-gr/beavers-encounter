using System;
using System.Collections.Generic;

namespace Beavers.Encounter.Core.DataInterfaces
{
    public static class BonusTasksRepositoryExtensions
    {
        /// <summary>
        /// Подсчет зачтенных бонусов.
        /// </summary>
        public static IEnumerable<BonusTask> AvailableNowTasks(this IList<BonusTask> bonusTasks)
        {
            foreach (BonusTask task in bonusTasks)
            {
                if (task.StartTime <= DateTime.Now && task.FinishTime >= DateTime.Now)
                {
                    yield return task;
                }
                    
            }
        }
    }
}
