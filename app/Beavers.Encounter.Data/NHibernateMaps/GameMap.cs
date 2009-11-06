using Beavers.Encounter.Core;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class GameMap : IAutoMappingOverride<Game>
    {
        public void Override(AutoMap<Game> mapping)
        {
            mapping.Map(x => x.GameState).CustomTypeIs(typeof (GameStates));

            mapping.HasMany(x => x.Teams);
            mapping.HasMany(x => x.Tasks);
            mapping.HasMany(x => x.BonusTasks);
        }
    }
}
