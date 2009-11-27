using Beavers.Encounter.Common.Filters;
using Beavers.Encounter.Core;
using SharpArch.Core.PersistenceSupport;

namespace Beavers.Encounter.Web.Controllers.Filters
{
    public class GameOwnerAttribute : FilterUsingAttribute
    {
        public GameOwnerAttribute()
            : base(typeof(GameOwnerFilter))
        {
            Order = 1;
        }
    }

    public class GameOwnerFilter : EntityOwnerFilter<Game, GamesController>
    {
        public GameOwnerFilter(IRepository<Game> repository)
            : base(repository)
        {
        }

        protected override int GetGameId(Game entity)
        {
            return entity.Id;
        }
    }
}
