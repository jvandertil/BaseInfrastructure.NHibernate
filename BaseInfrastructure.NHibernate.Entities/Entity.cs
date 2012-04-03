using System;

namespace BaseInfrastructure.NHibernate.Entities
{
    /// <summary>
    /// This class represents a basic Entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the inheriting class.</typeparam>
    public abstract class Entity<TEntity> where TEntity : Entity<TEntity>
    {
        public virtual Guid Id { get; private set; }

        private int? _oldHashCode;

        #region Equality Members
        public override int GetHashCode()
        {
            if (_oldHashCode.HasValue)
                return _oldHashCode.Value;

            var thisIsNew = Equals(Id, Guid.Empty);

            if (thisIsNew)
            {
                _oldHashCode = base.GetHashCode();

                return _oldHashCode.Value;
            }

            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as TEntity;
            if (other == null)
                return false;

            var thisIsNew = Equals(Id, Guid.Empty);
            var otherIsNew = Equals(other.Id, Guid.Empty);

            if (thisIsNew && otherIsNew)
                return ReferenceEquals(this, other);

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity<TEntity> lhs, Entity<TEntity> rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(Entity<TEntity> lhs, Entity<TEntity> rhs)
        {
            return !Equals(lhs, rhs);
        }
        #endregion
    }
}
