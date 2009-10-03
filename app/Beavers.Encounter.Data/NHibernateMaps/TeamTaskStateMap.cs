using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.Core;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;
using FluentNHibernate.Mapping;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TeamTaskStateMap : /*ClassMap<TeamTaskState>,*/ IAutoMappingOverride<TeamTaskState>
    {
        /*public TeamTaskStateMap()
        {
            WithTable("TeamTaskState");

            Id(x => x.Id)
                .ColumnName("Id")
                .WithUnsavedValue(0)
                .GeneratedBy.Identity();

            Map(x => x.TaskStartTime).Not.Nullable();
            Map(x => x.TaskFinishTime).Nullable();
            Map(x => x.State).Not.Nullable();

            References(x => x.TeamGameState)
                .ColumnName("TeamGameStateFk")
                .Not.Nullable();
            
            References(x => x.Task)
                .ColumnName("TaskFk")
                .Not.Nullable();
            
            References(x => x.NextTask)
                .ColumnName("NextTaskFk")
                .Nullable();

            HasMany(x => x.AcceptedTips);
            HasMany(x => x.AcceptedCodes);
        }*/

        public void Override(AutoMap<TeamTaskState> mapping)
        {
            //mapping.WithTable("TeamTaskState");
            mapping.References(x => x.NextTask)
                .WithColumns("NextTaskFk")
                .Nullable();

            mapping.HasMany(x => x.AcceptedTips);
            mapping.HasMany(x => x.AcceptedCodes);
            mapping.HasMany(x => x.AcceptedBadCodes);
        }
    }
}
