using System.Collections.ObjectModel;

namespace TestIdentity.SeedWork
{
    public class ReadOnlyCollection<T> : Collection<T>
        where T : Entity
    {
        public ReadOnlyCollection() : base() { }
        public ReadOnlyCollection(Collection<T> collection) : base(collection) { }
        public ReadOnlyCollection(ReadOnlyCollection<T> collection) : base(collection) { }

        [Obsolete("Add method throw a InvalidOperationException for ReadOnlyCollection", true)]
        new public void Add(T item) => throw new InvalidOperationException($"Add not allowed for entity type {typeof(T).FullName}");
        
        [Obsolete("Add method throw a InvalidOperationException for ReadOnlyCollection", true)]
        new public void Remove(T item) => throw new InvalidOperationException($"Remove not allowed for entity type {typeof(T).FullName}");
    }
}
