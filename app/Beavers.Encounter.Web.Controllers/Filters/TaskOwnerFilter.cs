using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class TaskOwnerAttribute : FilterUsingAttribute
    {
        public TaskOwnerAttribute()
            : base(typeof(TaskOwnerFilter))
        {
            Order = 1;
        }
    }

    public class TaskOwnerFilter : EntityOwnerFilter<Task, TasksController>
    {
        public TaskOwnerFilter(IRepository<Task> repository)
            : base(repository)
        {
        }

        protected override string GetEntityIdSpecificName()
        {
            return "taskId";
        }

        protected override int GetGameId(Task entity)
        {
            return entity.Game.Id;
        }
    }
}
