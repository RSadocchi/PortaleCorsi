using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public class PasswordHistoryConfiguration : IEntityTypeConfiguration<PasswordHistory>
    {
        public string Schema { get; set; }
        public bool SeedData { get; set; }
        public void Configure(EntityTypeBuilder<PasswordHistory> builder)
        {
            builder.ToTable(name: nameof(PasswordHistory), schema: Schema);
            builder.Property(e => e.UserId).HasColumnName("UserID");
            builder.HasOne(p => p.User).WithMany(p => p.PasswordHistories).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
