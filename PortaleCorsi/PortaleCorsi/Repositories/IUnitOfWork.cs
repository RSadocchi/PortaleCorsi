namespace PortaleCorsi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
