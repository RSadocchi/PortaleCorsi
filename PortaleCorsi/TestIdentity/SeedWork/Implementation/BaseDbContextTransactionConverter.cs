using Microsoft.EntityFrameworkCore.Storage;

namespace TestIdentity.SeedWork
{
    public class BaseDbContextTransactionConverter : IDbTransaction
    {
        readonly IDbContextTransaction _dbContextTransaction;

        public BaseDbContextTransactionConverter(IDbContextTransaction dbContextTransaction) { _dbContextTransaction = dbContextTransaction; }

        public void Commit() => _dbContextTransaction.Commit();

        public void Dispose() => _dbContextTransaction.Dispose();

        public void Rollback() => _dbContextTransaction.Rollback();
    }
}
