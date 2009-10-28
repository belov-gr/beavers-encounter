using Beavers.Encounter.Core;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TeamGameStateMap : IAutoMappingOverride<TeamGameState>
    {
        public void Override(AutoMap<TeamGameState> mapping)
        {
            mapping.References(x => x.ActiveTaskState)
                .WithColumns("TeamTaskStateFk")
                .Nullable();

            mapping.HasMany(x => x.AcceptedTasks).Inverse().Cascade.Delete();
        }
    }
}
