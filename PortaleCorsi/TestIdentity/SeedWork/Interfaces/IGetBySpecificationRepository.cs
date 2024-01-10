
namespace TestIdentity.SeedWork
{
    public interface IGetBySpecificationRepository<TEntity> where TEntity : Entity
    {
        IQueryable<T> GetBySpecification<T>(ISpecification<T>? spec = null, bool tracking = true, bool? splitQuery = null)
            where T : Entity;
    }
}
