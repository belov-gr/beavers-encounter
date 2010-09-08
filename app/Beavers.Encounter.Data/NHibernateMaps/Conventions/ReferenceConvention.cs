using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Beavers.Encounter.Data.NHibernateMaps.Conventions
{
    public class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(instance.Property.Name + "Fk");
        }
    }
}
