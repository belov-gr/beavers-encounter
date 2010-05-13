using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class BonusTaskOwnerAttribute : FilterUsingAttribute
    {
        public BonusTaskOwnerAttribute()
            : base(typeof(BonusTaskOwnerFilter))
        {
            Order = 1;
        }
    }

    public class BonusTaskOwnerFilter : EntityOwnerFilter<BonusTask, BonusTasksController>
    {
        public BonusTaskOwnerFilter(IRepository<BonusTask> repository)
            : base(repository)
        {
        }

        protected override string GetEntityIdSpecificName()
        {
            return "bonusId";
        }

        protected override int GetGameId(BonusTask entity)
        {
            return entity.Game.Id;
        }
    }
}
