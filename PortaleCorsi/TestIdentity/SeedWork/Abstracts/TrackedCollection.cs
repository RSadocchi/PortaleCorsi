using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TestIdentity.SeedWork
{
    public class TrackedCollection<T> : Collection<T>
        where T : TrackedEntity
    {
        public TrackedCollection() : base() { }
        public TrackedCollection(Collection<T> collection) : base(collection) { }
        public TrackedCollection(TrackedCollection<T> collection) : base(collection) { }

        private Dictionary<string, object?> _getContextKeys(T item)
            => item
                ?.GetType()
                ?.GetProperties()
                ?.Where(p => p.GetCustomAttributes(typeof(ContextKeyAttribute), false).Any())
                ?.OrderBy(a => a.Name)
                ?.ToDictionary(k => k.Name, v => v.GetValue(item))
            ?? throw new Exception($"{item?.GetType()?.FullName} does not have any {nameof(ContextKeyAttribute)}");

        private T _compareInCollection(T item)
        {
            var contextKeys = _getContextKeys(item: item);
            foreach (var other in base.Items)
                foreach (var prop in item.GetType().GetProperties().Where(t => contextKeys.ContainsKey(t.Name)))
                {
                    var otherKeys = _getContextKeys(item: other);
                    if (otherKeys.SequenceEqual(contextKeys)) return other;
                }
            return default(T);
        }

        new public void Add(T item)
        {
            if (!item.TrackedChanges.TryAdd(nameof(EntityState), EntityState.Added.ToString()))
                item.TrackedChanges[nameof(EntityState)] = EntityState.Added.ToString();
            base.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items?.Count() > 0)
                foreach (var item in items)
                    this.Add(item);
        }

        new public void Remove(T item) => this.Remove(item: item, realRemove: true);
        public void Remove(T item, bool realRemove)
        {
            if (realRemove) base.Remove(item);
            else
            {
                if (!item.TrackedChanges.TryAdd(nameof(EntityState), EntityState.Deleted.ToString()))
                    item.TrackedChanges[nameof(EntityState)] = EntityState.Deleted.ToString();
            }
        }
    }
}
