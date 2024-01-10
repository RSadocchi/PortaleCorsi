using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TestIdentity.SeedWork
{
    public abstract class TrackedEntity : Entity
    {
        [NotMapped]
        public Dictionary<string, string> TrackedChanges { get; set; } = new Dictionary<string, string>();

        public virtual void SetField<T>(ref T item, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                if (!TrackedChanges.ContainsKey(propertyName))
                    TrackedChanges.TryAdd(propertyName, !string.IsNullOrWhiteSpace(item?.ToString()) ?  item?.ToString() ?? "***" : "-");
                else
                    TrackedChanges[propertyName] = !string.IsNullOrWhiteSpace(item?.ToString()) ? item?.ToString() ?? "***" : "-";
            }
            item = value;
        }
    }
}
