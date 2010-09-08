using Beavers.Encounter.Core;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class UserMap : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            //mapping.Map(x => x.Password).ColumnName("PWD"); /* FireBird */
            mapping.IgnoreProperty(x => x.Identity);
            mapping.IgnoreProperty(x => x.IsAdministrator);
            mapping.IgnoreProperty(x => x.IsAuthor);
            mapping.IgnoreProperty(x => x.IsPlayer);
        }
    }
}
