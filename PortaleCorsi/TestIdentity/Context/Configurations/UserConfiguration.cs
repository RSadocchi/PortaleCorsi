using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(name: nameof(User), schema: Schema);
            builder.Property(e => e.Id).HasColumnName("UserID");
            builder.Property(e => e.Culture).IsRequired(false);
            builder.Property(e => e.PasswordHash).HasColumnType("ntext");

            if (SeedData)
            {
                if (SeedDataCommon.UserAdmin != null)
                    builder.HasData(SeedDataCommon.UserAdmin);
                if (SeedDataCommon.UserAuto != null)
                    builder.HasData(SeedDataCommon.UserAuto);
                if (SeedDataCommon.UserBatch != null)
                    builder.HasData(SeedDataCommon.UserBatch);
            }
        }
    }
}
