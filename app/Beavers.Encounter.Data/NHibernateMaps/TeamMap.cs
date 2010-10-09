using Beavers.Encounter.Core;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class TeamMap : IAutoMappingOverride<Team>
    {
        public void Override(AutoMapping<Team> mapping)
        {
            mapping.References(x => x.Game);
            mapping.References(x => x.TeamLeader);

            mapping.HasMany(x => x.TeamGameStates);
            mapping.HasMany(x => x.Users);

            mapping.HasManyToMany(x => x.PreventTasksAfterTeams)
                .Table("PreventTeams")
                .ChildKeyColumn("TeamFk")
                .ParentKeyColumn("TeamRefFk");
        }
    }
}
