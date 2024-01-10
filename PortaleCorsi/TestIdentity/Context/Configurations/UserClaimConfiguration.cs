using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable(name: nameof(UserClaim), schema: Schema);
            builder.Property(e => e.UserId).HasColumnName("UserID");
            builder.Property(e => e.Id).HasColumnName("UserClaimID");
            builder.Property(e => e.ClaimType).HasMaxLength(150).IsRequired();
            builder.Property(e => e.ClaimValue).IsRequired();

            if (SeedData)
                builder.HasData(new UserClaim() { Id = 1, UserId = SeedDataCommon.UserAdmin.Id, ClaimType = Const_ClaimTypes.GOD, ClaimValue = Const_ClaimValues.DefaultValueON });
        }
    }
}
