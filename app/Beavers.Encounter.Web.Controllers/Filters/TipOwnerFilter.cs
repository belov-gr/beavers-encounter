using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class TipOwnerAttribute : FilterUsingAttribute
    {
        public TipOwnerAttribute()
            : base(typeof(TipOwnerFilter))
        {
            Order = 1;
        }
    }

    public class TipOwnerFilter : EntityOwnerFilter<Tip, TipsController>
    {
        public TipOwnerFilter(IRepository<Tip> repository)
            : base(repository)
        {
        }

        protected override string GetEntityIdSpecificName()
        {
            return "tipId";
        }

        protected override int GetGameId(Tip entity)
        {
            return entity.Task.Game.Id;
        }
    }
}
