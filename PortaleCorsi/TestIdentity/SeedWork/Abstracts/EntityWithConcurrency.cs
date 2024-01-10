using System.ComponentModel.DataAnnotations;

namespace TestIdentity.SeedWork
{
    public abstract class EntityWithConcurrency : Entity
    {
        [Timestamp]
        public byte[] ConcurrencyTimestamp { get; set; }
    }
}
