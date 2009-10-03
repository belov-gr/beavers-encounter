using Castle.Windsor;
using SharpArch.Core.CommonValidator;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Core.PersistenceSupport;
using Tests.Beavers.Encounter.Data.TestDoubles;
using SharpArch.Data.NHibernate;
using Beavers.Encounter.Core.DataInterfaces;
using Beavers.Encounter.Data;

namespace Tests
{
    public class ServiceLocatorInitializer
    {
        public static void Init()
        {
            IWindsorContainer container = new WindsorContainer();
            container.AddComponent("validator",
                typeof(IValidator), typeof(Validator));
            container.AddComponent("entityDuplicateChecker",
                typeof(IEntityDuplicateChecker), typeof(EntityDuplicateCheckerStub));
            container.AddComponent("repositoryWithTypedId",
                typeof(IRepositoryWithTypedId<,>), typeof(RepositoryWithTypedId<,>));
            container.AddComponent("userRepository", typeof(IUserRepository), typeof(UserRepository));
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}
