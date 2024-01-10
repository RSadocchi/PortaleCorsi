using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class CaptchaConfiguration : IEntityTypeConfiguration<Captcha>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<Captcha> builder)
        {
            builder.ToTable(name: nameof(Captcha), schema: Schema);
            builder.Property(t => t.Validity).HasColumnType("datetime");
        }
    }
}
