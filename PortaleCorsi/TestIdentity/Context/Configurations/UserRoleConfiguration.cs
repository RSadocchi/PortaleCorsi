using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(name: nameof(UserRole), schema: Schema);
            builder.Property(e => e.UserId).HasColumnName("UserID");
            builder.Property(e => e.RoleId).HasColumnName("RoleID");

            if (SeedData && SeedDataCommon.UserAdmin != null)
            {
                if (SeedDataCommon.Roles?.Any(t => t.name == Const_Roles.Administrator) == true)
                    builder.HasData(new UserRole()
                    {
                        UserId = SeedDataCommon.UserAdmin.Id,
                        RoleId = SeedDataCommon.Roles.FirstOrDefault(t => t.name == Const_Roles.Administrator).id
                    });

                if (SeedDataCommon.Roles?.Any(t => t.name == Const_Roles.Backoffice_supervisor) == true)
                    builder.HasData(new UserRole()
                    {
                        UserId = SeedDataCommon.UserAdmin.Id,
                        RoleId = SeedDataCommon.Roles.FirstOrDefault(t => t.name == Const_Roles.Backoffice_supervisor).id
                    });

                if (SeedDataCommon.Roles?.Any(t => t.name == Const_Roles.Frontoffice_supervisor) == true)
                    builder.HasData(new UserRole()
                    {
                        UserId = SeedDataCommon.UserAdmin.Id,
                        RoleId = SeedDataCommon.Roles.FirstOrDefault(t => t.name == Const_Roles.Frontoffice_supervisor).id
                    });
            }
        }
    }
}
