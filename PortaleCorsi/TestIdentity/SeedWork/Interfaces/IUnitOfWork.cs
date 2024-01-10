using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestIdentity.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
        IDbTransaction BeginTransaction();
        void DetachAllEntities(bool includeAllEntriesStates = false);
        void Clear();
    }
}
