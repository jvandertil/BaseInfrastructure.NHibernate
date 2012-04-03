using System;
using NHibernate;

namespace BaseInfrastructure.NHibernate.Core
{
    /// <summary>
    /// Represents a basic Query.
    /// 
    /// You should not inherit this class, but instead use the Generic version.
    /// </summary>
    public abstract class Query
    {
        /// <summary>
        /// The NHibernate Unit of Work associated with this Query.
        /// </summary>
        public ISession Session { get; set; }

        /// <summary>
        /// A helper field for unit testing.
        /// </summary>
        public Func<Query, object> AlternativeQueryFunction { get; set; }

        /// <summary>
        /// Executes the given <see cref="Query"/> and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>The result of the query.</returns>
        protected TResult SubQuery<TResult>(Query<TResult> query)
        {
            if (AlternativeQueryFunction != null)
                return (TResult) AlternativeQueryFunction(query);

            return DefaultQuery(query);
        }

        /// <summary>
        /// Default executor for Queries.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the query.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>The result of the query.</returns>
        private TResult DefaultQuery<TResult>(Query<TResult> query)
        {
            query.Session = Session;
            return query.Execute();
        }
    }

    /// <summary>
    /// Represents a basic query that returns a generic result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class Query<TResult> : Query
    {
        public abstract TResult Execute();
    }
}