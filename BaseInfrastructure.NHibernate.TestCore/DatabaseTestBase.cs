using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace BaseInfrastructure.NHibernate.TestCore
{
    /// <summary>
    /// Provides a base class for tests that will hit a database.
    /// </summary>
    public abstract class DatabaseTestBase : TestBase
    {
        /// <summary>
        /// The NHibernate configuration.
        /// </summary>
        protected Configuration Configuration;

        /// <summary>
        /// The NHibernate SessionFactory used to build sessions.
        /// </summary>
        private ISessionFactory _sessionFactory;

        /// <summary>
        /// The current Unit of Work.
        /// </summary>
        protected ISession Session;

        protected override void BeforeAllTests()
        {
            base.BeforeAllTests();

            Configuration = Fluently.Configure()
                .Database(DefineDatabase)
                .Mappings(DefineMappings)
                .BuildConfiguration();

            _sessionFactory = Configuration.BuildSessionFactory();
        }

        /// <summary>
        /// Starts a new Unit of Work and returns it.
        /// </summary>
        /// <returns></returns>
        protected ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }

        protected override void Given()
        {
            base.Given();

            Session = OpenSession();
            CreateSchema();
        }

        /// <summary>
        /// This function should perform a database configuration using FluentNHibernate.
        /// </summary>
        /// <returns>The configuration for the database to be used.</returns>
        protected abstract IPersistenceConfigurer DefineDatabase();

        /// <summary>
        /// Adds the mappings to the NHibernate configuration.
        /// </summary>
        /// <param name="m"></param>
        protected abstract void DefineMappings(MappingConfiguration m);

        /// <summary>
        /// Initializes the database schema.
        /// </summary>
        protected virtual void CreateSchema() { }
    }
}