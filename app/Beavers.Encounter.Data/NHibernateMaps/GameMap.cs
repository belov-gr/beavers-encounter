using Beavers.Encounter.Core;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class GameMap : IAutoMappingOverride<Game>
    {
        public void Override(AutoMapping<Game> mapping)
        {
            mapping.Map(x => x.GameState).CustomType(typeof (GameStates));

            mapping.HasMany(x => x.Teams);
            mapping.HasMany(x => x.Tasks);
            mapping.HasMany(x => x.BonusTasks);
        }
    }
}
