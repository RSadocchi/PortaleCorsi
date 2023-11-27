using Microsoft.EntityFrameworkCore;

namespace PortaleCorsi.Repositories
{
    public class RepositoryBase<TDbContext, TEntity, TPrimaryKey> : IRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, new()
        where TDbContext : DbContext, IUnitOfWork
    {
        readonly TDbContext _context;
        public IUnitOfWork UnitOfWork { get => _context; }

        public RepositoryBase(TDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> FindAsync(TPrimaryKey id) => await _context.Set<TEntity>().FindAsync(id);

        public async Task<IQueryable<TEntity>> GetAsync() => await Task.FromResult(_context.Set<TEntity>().AsQueryable());

        public async Task<TEntity> AddAsync(TEntity entity) => await Task.FromResult(_context.Set<TEntity>().Add(entity).Entity);

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified; 
            return await Task.FromResult(entity);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await Task.CompletedTask;
        }

    }
}
