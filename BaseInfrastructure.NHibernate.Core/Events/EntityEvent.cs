using System;

namespace BaseInfrastructure.NHibernate.Core.Events
{
    /// <summary>
    /// A basic event that can be used to dispatch an entity to <seealso cref="EventProcessor"/>s.
    /// </summary>
    /// <typeparam name="TEntity">The type of the Entity</typeparam>
    public class EntityEvent<TEntity> : IObservable<TEntity>
    {
        private readonly TEntity _entity;

        /// <summary>
        /// Wrap an Entity into an event that can be dispatched.
        /// </summary>
        /// <param name="entity"></param>
        public EntityEvent(TEntity entity)
        {
            _entity = entity;
        }

        #region IObservable<TEntity> Members

        public IDisposable Subscribe(IObserver<TEntity> observer)
        {
            observer.OnNext(_entity);

            return null;
        }

        #endregion

    }
}