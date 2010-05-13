using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class GameboardOwnerAttribute : FilterUsingAttribute
    {
        public GameboardOwnerAttribute()
            : base(typeof(GameboardOwnerFilter))
        {
            Order = 1;
        }
    }

    public class GameboardOwnerFilter : EntityOwnerFilter<Game, GameboardController>
    {
        public GameboardOwnerFilter(IRepository<Game> repository)
            : base(repository)
        {
        }

        protected override string GetEntityIdSpecificName()
        {
            return "gameId";
        }

        protected override int GetGameId(Game entity)
        {
            return entity.Id;
        }
    }
}
