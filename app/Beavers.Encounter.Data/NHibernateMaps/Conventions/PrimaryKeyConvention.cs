using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Beavers.Encounter.Data.NHibernateMaps.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column("Id");
            instance.UnsavedValue("0");
            instance.GeneratedBy.Identity();
        }
    }
}
