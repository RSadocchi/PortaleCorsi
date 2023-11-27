namespace PortaleCorsi.Repositories
{
    public interface IRepositoryBase<TEntity, TPrimaryKey> where TEntity : class, new()
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TEntity?> FindAsync(TPrimaryKey id);
        Task<IQueryable<TEntity>> GetAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
    }
}
