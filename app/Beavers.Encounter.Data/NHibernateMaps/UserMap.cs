using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beavers.Encounter.Core;
using FluentNHibernate.AutoMap;
using FluentNHibernate.AutoMap.Alterations;

namespace Beavers.Encounter.Data
{
    public class UserMap : IAutoMappingOverride<User>
    {
        public void Override(AutoMap<User> mapping)
        {
            mapping.IgnoreProperty(x => x.Identity);
            mapping.IgnoreProperty(x => x.IsAdministrator);
            mapping.IgnoreProperty(x => x.IsAuthor);
            mapping.IgnoreProperty(x => x.IsPlayer);
        }
    }
}
