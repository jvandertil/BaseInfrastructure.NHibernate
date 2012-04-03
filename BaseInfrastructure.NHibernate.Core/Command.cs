using System;
using NHibernate;

namespace BaseInfrastructure.NHibernate.Core
{
    /// <summary>
    /// Represents a Command that does not produce output.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// The NHibernate Session (Unit of Work) that is used by this Command.
        /// </summary>
        public ISession Session { get; set; }

        /// <summary>
        /// This is a helper function to aid in Unit Testing.
        /// </summary>
        public Func<Query, object> AlternativeQueryFunction { get; set; }

        /// <summary>
        /// Executes the <see cref="Command"/>
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Executes the <see cref="Query"/> and returns the result of that <see cref="Query"/>.
        /// </summary>
        /// <typeparam name="TResult">The result type of the <see cref="Query"/></typeparam>
        /// <param name="query">The <see cref="Query"/> to execute.</param>
        /// <returns>The result of the <see cref="Query"/></returns>
        protected TResult Query<TResult>(Query<TResult> query)
        {
            if (AlternativeQueryFunction != null)
                return (TResult) AlternativeQueryFunction(query);

            return DefaultQuery(query);
        }

        /// <summary>
        /// The default executor for <see cref="Query"/> objects.
        /// </summary>
        /// <typeparam name="TResult">The result type of the <see cref="Query"/></typeparam>
        /// <param name="query">The <see cref="Query"/> to execute.</param>
        /// <returns>The result of the <see cref="Query"/></returns>
        private TResult DefaultQuery<TResult>(Query<TResult> query)
        {
            query.Session = Session;
            return query.Execute();
        }
    }

    /// <summary>
    /// Represents a <see cref="Command"/> that produces output.
    /// </summary>
    /// <typeparam name="TResult">The type of the output</typeparam>
    public abstract class Command<TResult> : Command
    {
        /// <summary>
        /// The output of this <see cref="Command"/>.
        /// </summary>
        public TResult Result { get; protected set; }
    }
}