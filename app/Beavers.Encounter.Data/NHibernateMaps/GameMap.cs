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
    public class GameMap : IAutoMappingOverride<Game>
    {
        public void Override(AutoMap<Game> mapping)
        {
            mapping.HasMany(x => x.Teams);
            mapping.HasMany(x => x.Tasks);
            mapping.HasMany(x => x.BonusTasks);
        }
    }
}
