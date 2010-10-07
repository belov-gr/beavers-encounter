using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

using Beavers.Encounter.Core;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class RoleMap : IAutoMappingOverride<Role>
    {
        public void Override(AutoMapping<Role> mapping)
        {
            mapping.IgnoreProperty(x => x.IsAdministrator);
            mapping.IgnoreProperty(x => x.IsAuthor);
            mapping.IgnoreProperty(x => x.IsGuest);
            mapping.IgnoreProperty(x => x.IsPlayer);
            mapping.IgnoreProperty(x => x.IsTeamLeader);
        }
    }
}
