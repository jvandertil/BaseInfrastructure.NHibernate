using FluentNHibernate.Cfg.Db;

namespace BaseInfrastructure.NHibernate.TestCore
{
    /// <summary>
    /// Provides a test base for tests that will use a SQLite In-Memory database.
    /// 
    /// Be sure to override the CreateSchema method!
    /// </summary>
    public abstract class SQLiteDatabaseTestBase : DatabaseTestBase
    {
        protected override IPersistenceConfigurer DefineDatabase()
        {
            return DefineSQLiteDatabase();
        }

        /// <summary>
        /// Sets up a SQLite In-Memory database.
        /// </summary>
        /// <returns></returns>
        private IPersistenceConfigurer DefineSQLiteDatabase()
        {
            return SQLiteConfiguration.Standard
                .InMemory()
                .Raw("hbm2ddl.keywords", "auto-quote")
                .ShowSql().FormatSql();
        }
    }
}