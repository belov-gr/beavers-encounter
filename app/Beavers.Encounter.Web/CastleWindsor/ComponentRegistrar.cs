using Beavers.Encounter.ApplicationServices;
using Beavers.Encounter.Web.Controllers.Filters;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Data.NHibernate;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.Castle;
using Castle.MicroKernel.Registration;
using SharpArch.Core.CommonValidator;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;

namespace Beavers.Encounter.Web.CastleWindsor
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddGenericRepositoriesTo(container);
            AddCustomRepositoriesTo(container);
            AddApplicationServicesTo(container);

            container.Register(
                //Component.For<IUnitOfWorkManager>().ImplementedBy<LinqToSqlUnitOfWorkManager>().LifeStyle.Transient,
                //Component.For<IFormsAuthentication>().ImplementedBy<FormsAuthenticationWrapper>(),
                Component.For<IServiceLocator>().Instance(new WindsorServiceLocator(container)),
                Component.For<AuthenticateFilter>().LifeStyle.Transient,
                Component.For<LockIfGameStartFilter>().LifeStyle.Transient,
                Component.For<GameStateFilter>().LifeStyle.Transient,
                Component.For<TeamGameboardFilter>().LifeStyle.Transient
                //Component.For<UnitOfWorkFilter>().LifeStyle.Transient,
                //Component.For<DataBinder>().LifeStyle.Transient,
                //Component.For<LoadUsingFilter>().LifeStyle.Transient,
                //Component.For<CurrentBasketBinder>().LifeStyle.Transient,
                //Component.For<ProductBinder>().LifeStyle.Transient,
                //Component.For<OrderBinder>().LifeStyle.Transient,
                //Component.For<IOrderSearchService>().ImplementedBy<OrderSearchService>().LifeStyle.Transient,
                //Component.For<IEmailBuilder>().ImplementedBy<EmailBuilder>().LifeStyle.Singleton
            );

            container.AddComponent("validator",
                typeof(IValidator), typeof(Validator));
        }

        private static void AddApplicationServicesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes.Pick()
                .FromAssemblyNamed("Beavers.Encounter.ApplicationServices")
                .WithService.FirstInterface());
        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes.Pick()
                .FromAssemblyNamed("Beavers.Encounter.Data")
                .WithService.FirstNonGenericCoreInterface("Beavers.Encounter.Core"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.AddComponent("entityDuplicateChecker",
                typeof(IEntityDuplicateChecker), typeof(EntityDuplicateChecker));
            container.AddComponent("repositoryType",
                typeof(IRepository<>), typeof(Repository<>));
            container.AddComponent("nhibernateRepositoryType",
                typeof(INHibernateRepository<>), typeof(NHibernateRepository<>));
            container.AddComponent("repositoryWithTypedId",
                typeof(IRepositoryWithTypedId<,>), typeof(RepositoryWithTypedId<,>));
            container.AddComponent("nhibernateRepositoryWithTypedId",
                typeof(INHibernateRepositoryWithTypedId<,>), typeof(NHibernateRepositoryWithTypedId<,>));
        }
    }
}
