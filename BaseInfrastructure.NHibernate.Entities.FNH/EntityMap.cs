using FluentNHibernate.Mapping;

namespace BaseInfrastructure.NHibernate.Entities
{
    public class EntityMap<TEntity> : ClassMap<TEntity> where TEntity : Entity<TEntity>
    {
        public EntityMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
        }
    }
}
