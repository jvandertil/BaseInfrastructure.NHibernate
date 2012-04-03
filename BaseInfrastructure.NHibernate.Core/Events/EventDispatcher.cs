using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NHibernate;

namespace BaseInfrastructure.NHibernate.Core.Events
{
    /// <summary>
    /// This class can be used to dispatch events to all the <see cref="EventProcessor"/>s in the domain.
    /// </summary>
    public static class EventDispatcher
    {
        /// <summary>
        /// A threadsafe collection of hinted assemblies.
        /// </summary>
        private static readonly ConcurrentBag<Assembly> HintedAssemblies = new ConcurrentBag<Assembly>();

        /// <summary>
        /// Hint the dispatcher that this assembly contains <see cref="EventProcessor"/>s.
        /// If you hint assemblies, the dispatcher will only use the hinted assemblies. 
        /// 
        /// If no assemblies are hinted to the dispatcher, it will search through the current <see cref="AppDomain"/> for <see cref="EventProcessor"/>s.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> containing <see cref="EventProcessor"/>s.</param>
        public static void AssemblyHint(Assembly assembly)
        {
            HintedAssemblies.Add(assembly);
        }

        /// <summary>
        /// Dispatch an <seealso cref="IObservable{T}"/>"/> to all the <see cref="EventProcessor"/>s.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IObservable{T}"/></typeparam>
        /// <param name="observable">The event to dispatch.</param>
        /// <param name="session">The NHibernate Unit of Work associated with this event.</param>
        public static void DispatchEvent<T>(IObservable<T> observable, ISession session) where T : class
        {
            IEnumerable<Assembly> assemblies;
            if (!HintedAssemblies.Any())
            {
                //Ask the current AppDomain for all assemblies.
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            else
            {
                //Use only the hinted assemblies.
                assemblies = HintedAssemblies;
            }

            //Search for EventProcessors, but do not include abstract EventProcessors.
            var eventProcessors = from x in assemblies.SelectMany(x => x.GetTypes())
                                  where !x.IsAbstract && typeof(EventProcessor).IsAssignableFrom(x)
                                  select x;

            foreach (Type eventProcessor in eventProcessors)
            {
                Trace.TraceInformation("calling event processor: {0}", eventProcessor.Name);

                var instance = (EventProcessor)Activator.CreateInstance(eventProcessor);

                Trace.TraceInformation("Instantiated {0}", eventProcessor.Name);

                instance.Session = session;

                Trace.TraceInformation("Sending event to {0}", eventProcessor.Name);
                instance.Execute(observable);
            }
        }
    }
}