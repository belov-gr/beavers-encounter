using System;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using Beavers.Encounter.Core;
using Beavers.Encounter.Data.NHibernateMaps.Conventions;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace Beavers.Encounter.Data.NHibernateMaps
{
    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        public AutoPersistenceModel Generate()
        {
            return AutoMap.AssemblyOf<User>(new BeaversEncounterMappingConfiguration())
                .Conventions.Setup(GetConventions())
                .IgnoreBase<Entity>()
                .IgnoreBase(typeof(EntityWithTypedId<>))
                .UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
        }

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
                c.Add<PrimaryKeyConvention>();
                c.Add<ReferenceConvention>();
                c.Add<HasManyConvention>();
                c.Add<TableNameConvention>();
            };
        }
    }
}
