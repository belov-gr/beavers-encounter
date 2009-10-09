using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.Core;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TeamMap : IAutoMappingOverride<Team>
    {
        public void Override(AutoMap<Team> mapping)
        {
            mapping.References(x => x.Game, "Id")
                .WithForeignKey();

            mapping.References(x => x.TeamLeader);

            mapping.HasMany(x => x.TeamGameStates);
            mapping.HasMany(x => x.Users);

            mapping.HasManyToMany(x => x.PreventTasksAfterTeams)
                .WithTableName("PreventTeams")
                .WithChildKeyColumn("TeamFk")
                .WithParentKeyColumn("TeamRefFk");
        }
    }
}
