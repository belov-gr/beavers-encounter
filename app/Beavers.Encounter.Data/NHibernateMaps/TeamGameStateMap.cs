using Beavers.Encounter.Core;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TeamGameStateMap : IAutoMappingOverride<TeamGameState>
    {
        public void Override(AutoMapping<TeamGameState> mapping)
        {
            mapping.References(x => x.ActiveTaskState)
                .Columns("TeamTaskStateFk")
                .Nullable();

            mapping.HasMany(x => x.AcceptedTasks).Inverse().Cascade.Delete();
        }
    }
}
