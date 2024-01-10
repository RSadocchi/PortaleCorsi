using System.Collections.ObjectModel;

namespace TestIdentity.SeedWork
{
    public class ValidatableCollection<T> : Collection<T>
        where T : Entity
    {
        public ValidatableCollection() : base() { }
        public ValidatableCollection(Collection<T> collection) : base(collection) { }
        public ValidatableCollection(ValidatableCollection<T> collection) : base(collection) { }

        [Obsolete("Must use Add with predicate!", true)]
        new public void Add(T item) => throw new InvalidOperationException("Must use Add with predicate!");

        /// <summary>
        /// Try to add a item to collectin, if predicate is true
        /// </summary>
        /// <param name="item">Element to try add to collection</param>
        /// <param name="predicate">If true item is add to collection</param>
        /// <param name="throwExceptionIfFalse">If true and predicate is false throw a InvalidOperationException, otherwise do nothing</param>
        public void Add(T item, Func<IEnumerable<T>, bool> predicate, bool throwExceptionIfFalse)
        {
            if (predicate.Invoke(base.Items)) base.Add(item);
            else if (throwExceptionIfFalse) throw new InvalidOperationException();
        }
    }
}
