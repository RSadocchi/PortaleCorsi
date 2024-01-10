using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable(name: nameof(UserLogin), schema: Schema);
            builder.Property(e => e.UserId).HasColumnName("UserID");
        }
    }
}
