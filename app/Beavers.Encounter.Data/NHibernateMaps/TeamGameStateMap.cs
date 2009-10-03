using Beavers.Encounter.Core;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;
using FluentNHibernate.Mapping;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TeamGameStateMap : /*ClassMap<TeamGameState>,*/ IAutoMappingOverride<TeamGameState>
    {
        /*public TeamGameStateMap()
        {
            WithTable("TeamGameState");
            
            Id(x => x.Id).WithUnsavedValue(0)
                .GeneratedBy.Identity();

            Map(x => x.GameDoneTime).Nullable();
            
            References(x => x.Team)
                .ColumnName("TeamFk")
                .Not.Nullable();
            
            References(x => x.Game)
                .ColumnName("GameFk")
                .Not.Nullable();

            References(x => x.ActiveTaskState)
                .ColumnName("TeamTaskStateFk")
                .Nullable();
        }
        */
        public void Override(AutoMap<TeamGameState> mapping)
        {
            mapping.References(x => x.ActiveTaskState)
                .WithColumns("TeamTaskStateFk")
                .Nullable();

            mapping.HasMany(x => x.AcceptedTasks);
        }
    }
}
