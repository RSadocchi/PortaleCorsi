using Microsoft.EntityFrameworkCore;

namespace TestIdentity.SeedWork
{
    public abstract class BaseGetBySpecificationRepository<TDbContext, TEntity> : IGetBySpecificationRepository<TEntity>
        where TEntity : Entity
        where TDbContext : DbContext, IUnitOfWork
    {
        protected readonly TDbContext _context;
        protected bool _defaultSplitQuery;

        public BaseGetBySpecificationRepository(
            TDbContext context,
            bool defaultSplitQuery = false)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _defaultSplitQuery = defaultSplitQuery;
        }

        public virtual IQueryable<T> GetBySpecification<T>(ISpecification<T>? spec = null, bool tracking = true, bool? splitQuery = null)
            where T : Entity
        {
            var query = _context.Set<T>().Select(r => r);
            if (tracking == false) query = query.AsNoTracking();
            if ((splitQuery ?? _defaultSplitQuery) == true) query = query.AsSplitQuery();
            if (spec != null) query = query.Where(spec.Expression);
            return query;
        }
    }
}
