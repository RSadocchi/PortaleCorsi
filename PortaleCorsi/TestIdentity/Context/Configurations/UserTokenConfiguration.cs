using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable(name: nameof(UserToken), schema: Schema);
            builder.Property(e => e.UserId).HasColumnName("UserID");
            builder.HasOne(p => p.User).WithMany(p => p.UserTokens).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
