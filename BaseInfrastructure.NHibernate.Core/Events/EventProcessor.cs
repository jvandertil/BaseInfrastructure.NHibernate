using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using NHibernate;

namespace BaseInfrastructure.NHibernate.Core.Events
{
    /// <summary>
    /// A base class for handling events that are fired from the domain.
    /// </summary>
    public abstract class EventProcessor
    {
        private readonly IList<Func<IObservable<object>, IObservable<object>>> _actions = new List<Func<IObservable<object>, IObservable<object>>>();

        /// <summary>
        /// The NHibernate Unit of Work to use.
        /// </summary>
        public ISession Session { get; set; }

        /// <summary>
        /// Adds an action to perform when an event arrives.
        /// </summary>
        /// <typeparam name="T">Type of the event</typeparam>
        /// <param name="action">The action to perform when the event happens.</param>
        protected void On<T>(Func<IObservable<T>, IObservable<object>> action)
        {
            _actions.Add(observable => action(observable.OfType<T>()));
        }

        /// <summary>
        /// Executes an event on all the defined actions
        /// </summary>
        /// <param name="observable">The event to execute.</param>
        public void Execute(IObservable<object> observable)
        {
            foreach (var action in _actions)
            {
                action(observable).Subscribe();
            }
        }
    }
}