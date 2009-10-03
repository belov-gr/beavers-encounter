using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.Core;
using Beavers.Encounter.Core.DataInterfaces;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;

namespace Beavers.Encounter.Data
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public IList<Task> GetByGame(Game game)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(Task))
                .Add(Expression.Eq("Game", game));

            IList<Task> taskList = criteria.List<Task>();
            return taskList as List<Task>;
        }
    }
}
