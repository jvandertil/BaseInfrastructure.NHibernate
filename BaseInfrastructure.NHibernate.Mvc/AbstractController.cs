using System;
using System.Web;
using System.Web.Mvc;
using BaseInfrastructure.NHibernate.Core;
using BaseInfrastructure.NHibernate.Core.Events;
using NHibernate;

namespace BaseInfrastructure.NHibernate.Mvc
{
    /// <summary>
    /// A Controller that has access to the infrastructure.
    /// </summary>
    public abstract class AbstractController : Controller
    {
        public HttpSessionStateBase HttpSession { get { return base.Session; } }

        /// <summary>
        /// The current NHibernate Unit of Work
        /// </summary>
        public new ISession Session { get; set; }

        #region Unit Testing functions
        /// <summary>
        /// A function to assist in unit testing. 
        /// 
        /// Can be used to capture <see cref="Command"/>s when they are executed.
        /// </summary>
        public Action<Command> AlternativeExecuteCommand { get; set; }

        /// <summary>
        /// A function to assist in unit testing.
        /// 
        /// Can be used to capture events when they are dispatched.
        /// </summary>
        public Action<IObservable<object>> AlternativeRaiseEvent { get; set; }

        /// <summary>
        /// A function to assist in unit testing.
        /// 
        /// Can be used to capture <see cref="Command{TResult}"/>s when they are executed.
        /// </summary>
        public Func<Command, object> AlternativeExecuteCommandWithResult { get; set; }

        /// <summary>
        /// A function to assist in unit testing.
        /// 
        /// Can be used to capture <see cref="Query{TResult}"/>s when they are executed.
        /// </summary>
        public Func<Query, object> AlternativeQueryFunction { get; set; }
        #endregion

        #region Infrastructure functions
        /// <summary>
        /// Executes a <see cref="Command"/> that does not return a result.
        /// </summary>
        /// <param name="cmd">The <see cref="Command"/> to execute.</param>
        protected void ExecuteCommand(Command cmd)
        {
            if (AlternativeExecuteCommand != null)
                AlternativeExecuteCommand(cmd);
            else
                DefaultExecuteCommand(cmd);
        }

        /// <summary>
        /// Raises an event that is dispatched through the domain.
        /// </summary>
        /// <typeparam name="T">The type of the event</typeparam>
        /// <param name="event">The event to dispatch.</param>
        protected void Raise<T>(IObservable<T> @event) where T : class
        {
            if (AlternativeRaiseEvent != null)
                AlternativeRaiseEvent(@event);
            else
                EventDispatcher.DispatchEvent(@event, Session);
        }

        /// <summary>
        /// Executes a <see cref="Query{TResult}"/> and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The <see cref="Query{TResult}"/>  to execute.</param>
        /// <returns>The result of the <see cref="Query{TResult}"/>.</returns>
        protected TResult Query<TResult>(Query<TResult> query)
        {
            if (AlternativeQueryFunction != null)
                return (TResult)AlternativeQueryFunction(query);

            return DefaultQuery(query);
        }

        /// <summary>
        /// Executes a <see cref="Command{TResult}"/> and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="cmd">Thee <see cref="Command{TResult}"/> to execute.</param>
        /// <returns>The result of the command.</returns>
        protected TResult ExecuteCommand<TResult>(Command<TResult> cmd)
        {
            if (AlternativeExecuteCommandWithResult != null)
                return (TResult)AlternativeExecuteCommandWithResult(cmd);

            return DefaultExecuteCommand(cmd);
        }
        #endregion

        #region Default query and command functions
        private TResult DefaultQuery<TResult>(Query<TResult> query)
        {
            query.Session = Session;

            return query.Execute();
        }

        private void DefaultExecuteCommand(Command cmd)
        {
            cmd.Session = Session;
            cmd.Execute();
        }

        private TResult DefaultExecuteCommand<TResult>(Command<TResult> cmd)
        {
            ExecuteCommand((Command)cmd);

            return cmd.Result;
        }
        #endregion
    }
}
