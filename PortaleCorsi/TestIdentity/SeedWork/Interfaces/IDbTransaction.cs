using System;

namespace TestIdentity.SeedWork
{
    public interface IDbTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
