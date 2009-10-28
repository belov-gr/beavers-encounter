using Beavers.Encounter.Core;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TeamTaskStateMap : IAutoMappingOverride<TeamTaskState>
    {
        public void Override(AutoMap<TeamTaskState> mapping)
        {
            mapping.References(x => x.NextTask)
                .WithColumns("NextTaskFk")
                .Nullable();

            mapping.HasMany(x => x.AcceptedTips).Inverse().Cascade.Delete();
            mapping.HasMany(x => x.AcceptedCodes).Inverse().Cascade.Delete();
            mapping.HasMany(x => x.AcceptedBadCodes).Inverse().Cascade.Delete();
        }
    }
}
