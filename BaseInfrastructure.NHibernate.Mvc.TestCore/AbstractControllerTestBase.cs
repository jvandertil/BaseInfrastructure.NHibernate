using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BaseInfrastructure.NHibernate.Core;
using Moq;
using NHibernate;

namespace BaseInfrastructure.NHibernate.Mvc.TestCore
{
    /// <summary>
    /// Provides a testing base that can be used to test classes deriving from the AbstractController class.
    /// 
    /// This base will mock and fake the entire infrastructure so that the controller can be tested in isolation.
    /// </summary>
    public abstract class AbstractControllerTestBase : ControllerTestBase
    {
        /// <summary>
        /// The mocked NHibernate Unit of Work.
        /// </summary>
        protected Mock<ISession> SessionMock;

        /// <summary>
        /// A list of <see cref="Command"/>s that were executed by the controller.
        /// </summary>
        protected IList<Command> ExecutedCommands;

        /// <summary>
        /// A list of <see cref="Query"/> objects that were executed by the controller.
        /// </summary>
        protected IList<Query> ExecutedQueries;

        /// <summary>
        /// A list of events raised by the controller.
        /// </summary>
        protected IList<IObservable<object>> RaisedEvents;

        /// <summary>
        /// A dictionary mapping a <see cref="Query"/> type to a result.
        /// </summary>
        protected IDictionary<Type, object> QueryResponses;

        /// <summary>
        /// A dictionary mapping a <see cref="Command"/> type to a result.
        /// </summary>
        protected IDictionary<Type, object> CommandResponses;

        protected override void Given()
        {
            base.Given();

            ExecutedCommands = new List<Command>();
            ExecutedQueries = new List<Query>();
            QueryResponses = new Dictionary<Type, object>();
            CommandResponses = new Dictionary<Type, object>();
            RaisedEvents = new List<IObservable<object>>();

            SessionMock = new Mock<ISession>();
        }

        /// <summary>
        /// Executes the specified action on the given <see cref="Controller"/>.
        /// </summary>
        /// <typeparam name="TController">The <see cref="Controller"/> to use.</typeparam>
        /// <param name="action">The action to invoke.</param>
        /// <returns>The result of the invoked action.</returns>
        protected ActionResult ExecuteAction<TController>(Func<TController, ActionResult> action)
            where TController : AbstractController
        {
            var controller = (TController)Activator.CreateInstance(typeof(TController));

            controller.LazySession = new Lazy<ISession>(() => SessionMock.Object);
            controller.AlternativeExecuteCommand = CaptureCommand;
            controller.AlternativeExecuteCommandWithResult = CaptureCommandWithResult;
            controller.AlternativeQueryFunction = CaptureQuery;
            controller.AlternativeRaiseEvent = CaptureEvent;

            SetFakeControllerContext(controller);

            SetupController(controller);

            return action.Invoke(controller);
        }

        /// <summary>
        /// This function can be used to perform additional setup on the controller before the action is invoked.
        /// </summary>
        /// <param name="controller">The controller that will be used to invoke an action.</param>
        protected virtual void SetupController(AbstractController controller)
        {
        }

        /// <summary>
        /// Set the response that is returned when the specified <see cref="Query"/> is captured.
        /// </summary>
        /// <typeparam name="TQuery">The type of the <see cref="Query"/>.</typeparam>
        /// <param name="response">The result of the <see cref="Query"/> that is captured.</param>
        protected void SetupQueryResult<TQuery>(object response)
        {
            QueryResponses.Add(typeof(TQuery), response);
        }

        /// <summary>
        /// Sets the response that is returned when the specified <see cref="Command"/> is captured.
        /// </summary>
        /// <typeparam name="TCommand">The type of the <see cref="Command"/>.</typeparam>
        /// <param name="result">The result of the <see cref="Command"/> that is captured.</param>
        protected void SetupCommandResult<TCommand>(object result)
        {
            CommandResponses.Add(typeof(TCommand), result);
        }

        #region Alternative commands for AbstractControllers
        private void CaptureCommand(Command command)
        {
            ExecutedCommands.Add(command);
        }

        private object CaptureCommandWithResult(Command command)
        {
            CaptureCommand(command);

            return CommandResponses[command.GetType()];
        }

        private object CaptureQuery(Query query)
        {
            ExecutedQueries.Add(query);

            return QueryResponses[query.GetType()];
        }

        private void CaptureEvent(IObservable<object> @event)
        {
            RaisedEvents.Add(@event);
        }
        #endregion
    }
}