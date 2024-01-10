using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIdentity.SeedWork
{
    public abstract class BaseRepository<TDbContext, TEntity, TPrimaryKey> : BaseGetBySpecificationRepository<TDbContext, TEntity>, IRepository<TEntity, TPrimaryKey>
        where TEntity : Entity, IAggregateRoot
        where TDbContext : DbContext, IUnitOfWork
    {
        public IUnitOfWork UnitOfWork { get => _context; }

        public BaseRepository(TDbContext context, bool defaultSplitQuery = false) : base(context: context, defaultSplitQuery: defaultSplitQuery) { }

        public virtual TEntity? Find(TPrimaryKey id) => _context.Set<TEntity>().Find(id);
        public virtual async Task<TEntity?> FindAsync(TPrimaryKey id) => await _context.Set<TEntity>().FindAsync(id);

        public virtual IQueryable<TEntity> GetAll() => _context.Set<TEntity>().AsQueryable();
        public virtual async Task<IQueryable<TEntity>> GetAllAsync() => await Task.FromResult(_context.Set<TEntity>().AsQueryable());

        public virtual TEntity Add(TEntity entity) { return _context.Set<TEntity>().Add(entity).Entity; }
        public virtual Task<TEntity> AddAsync(TEntity entity) { return Task.FromResult(Add(entity)); }

        public virtual void AddRange(IEnumerable<TEntity> entities) { _context.Set<TEntity>().AddRange(entities); }
        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities) { AddRange(entities); return Task.CompletedTask; }

        public virtual TEntity Update(TEntity entity) { _context.Entry(entity).State = EntityState.Modified; return entity; }
        public virtual Task<TEntity> UpdateAsync(TEntity entity) { return Task.FromResult(Update(entity)); }

        public virtual void Remove(TEntity entity) { _context.Set<TEntity>().Remove(entity); }
        public virtual Task RemoveAsync(TEntity entity) { Remove(entity); return Task.CompletedTask; }

        public virtual void RemoveRange(IEnumerable<TEntity> entities) { _context.Set<TEntity>().RemoveRange(entities); }
        public virtual Task RemoveRangeAsync(IEnumerable<TEntity> entities) { RemoveRange(entities); return Task.CompletedTask; }

        public virtual TEntity Attach(TEntity entity) { return _context.Set<TEntity>().Attach(entity).Entity; }
        public virtual Task<TEntity> AttachAsync(TEntity entity) { return Task.FromResult(Attach(entity)); }

        public virtual void Detach(TEntity entity) { _context.Entry(entity).State = EntityState.Detached; }
        public virtual Task DetachAsync(TEntity entity) { Detach(entity); return Task.CompletedTask; }
    }
}
