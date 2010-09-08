using Beavers.Encounter.Core;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TeamTaskStateMap : IAutoMappingOverride<TeamTaskState>
    {
        public void Override(AutoMapping<TeamTaskState> mapping)
        {
            mapping.References(x => x.NextTask)
                .Columns("NextTaskFk")
                .Nullable();

            mapping.HasMany(x => x.AcceptedTips).Inverse().Cascade.Delete();
            mapping.HasMany(x => x.AcceptedCodes).Inverse().Cascade.Delete();
            mapping.HasMany(x => x.AcceptedBadCodes).Inverse().Cascade.Delete();
        }
    }
}
