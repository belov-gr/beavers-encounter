using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Core.DataInterfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
        IList<Task> GetByGame(Game game);
    }
}
