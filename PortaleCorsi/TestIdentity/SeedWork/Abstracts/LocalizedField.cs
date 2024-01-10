using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace TestIdentity.SeedWork
{
    public class LocalizedItem
    {
        public string L { get; set; }
        public string V { get; set; }
    }

    public class LocalizedField
    {
        public List<LocalizedItem> D_a_t_a { get; set; } = new List<LocalizedItem>();

        public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
    }

    public static class LocalizedFieldExtensions
    {
        public static string? Locale(this LocalizedField source, string lang = null)
        {
            lang ??= System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            return source?.D_a_t_a
                ?.Where(t => t.L.ToLower().Trim().StartsWith(lang.ToLower().Trim()) || lang.ToLower().Trim().StartsWith(t.L.ToLower().Trim()))
                ?.FirstOrDefault()
                ?.V;
        }

        public static void UpdateValue(this LocalizedField source, string culture, string value)
        {
            if (source == null) return;
            if (!(source.D_a_t_a?.Count > 0)) source.D_a_t_a = new List<LocalizedItem>();
            if (source.D_a_t_a.Any(t => t.L.Equals(culture, StringComparison.InvariantCulture)))
                source.D_a_t_a.FirstOrDefault(t => t.L.Equals(culture, StringComparison.InvariantCulture)).V = value;
            else
                source.D_a_t_a.Add(new LocalizedItem()
                {
                    L = culture,
                    V = value
                });
        }

        public static ValueConverter<LocalizedField, string> LocalizationModelConverter => new ValueConverter<LocalizedField, string>(
            s => s != null && s.D_a_t_a != null && s.D_a_t_a.Count > 0 ?
                JsonSerializer.Serialize(s, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }) : null,
            g => !string.IsNullOrWhiteSpace(g) ?
                JsonSerializer.Deserialize<LocalizedField>(g, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }) : new LocalizedField());

        public static ValueComparer<LocalizedField> LocalizationModelComparer => new ValueComparer<LocalizedField>(
            (a, b) => a != null && b != null && a.D_a_t_a != null && b.D_a_t_a != null
                && a.D_a_t_a.OrderBy(t => t.L).ThenBy(t => t.V).SequenceEqual(b.D_a_t_a.OrderBy(t => t.L).ThenBy(t => t.V)),
            t => t.D_a_t_a.Aggregate(0, (t1, t2) => HashCode.Combine(t1, t2.GetHashCode())),
            t => t);
    }
}
