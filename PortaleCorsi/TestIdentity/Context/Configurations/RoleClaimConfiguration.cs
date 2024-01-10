using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable(name: nameof(RoleClaim), schema: Schema);
            builder.Property(e => e.Id).HasColumnName("RoleClaimID");
            builder.Property(e => e.RoleId).HasColumnName("RoleID");
            builder.Property(e => e.ClaimType).HasMaxLength(50).IsRequired();
            builder.Property(e => e.ClaimValue).HasMaxLength(50).IsRequired();

            if (SeedData)
            {
                int _id = 0;
                if (SeedDataCommon.Roles?.Any() == true)
                    foreach (var role in SeedDataCommon.Roles)
                        if (role.claims?.Any() == true)
                            foreach (var claim in role.claims)
                            {
                                _id += 1;
                                builder.HasData(new RoleClaim()
                                {
                                    Id = _id,
                                    RoleId = role.id,
                                    ClaimType = claim,
                                    ClaimValue = Const_ClaimValues.DefaultValueON
                                });
                            }
            }
        }
    }
}
