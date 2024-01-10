using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(name: nameof(Role), schema: Schema);
            builder.Property(e => e.Id).HasColumnName("RoleID");

            if (SeedData)
                if (SeedDataCommon.Roles?.Any() == true)
                    builder.HasData(SeedDataCommon.Roles
                        .Select(t => new Role()
                        {
                            Id = t.id,
                            Name = t.name,
                            NormalizedName = t.name.ToUpper()
                        }));
        }
    }
}
