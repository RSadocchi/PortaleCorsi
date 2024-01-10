using TestIdentity.SeedWork;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public abstract class BaseIdentityDbContext<TDbCtx> : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IIdentityDbContext
        where TDbCtx : DbContext, IUnitOfWork
    {
        public string? ConnectionString { get; set; }
        public bool SeedDataCommon { get; set; }

        public DbSet<Captcha> Captchas { get; set; }
        public DbSet<PasswordHistory> PasswordHistory { get; set; }

        public BaseIdentityDbContext() : base() { }
        public BaseIdentityDbContext(DbContextOptions<TDbCtx> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            bool sensitiveDataLoggingEnabled = false;
            bool detailedErrorsEnabled = false;
            if (System.Diagnostics.Debugger.IsAttached)
            {
                sensitiveDataLoggingEnabled = true;
                detailedErrorsEnabled = true;
            }
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: sensitiveDataLoggingEnabled)
                    .EnableDetailedErrors(detailedErrorsEnabled: detailedErrorsEnabled)
                    .UseSqlServer(connectionString: ConnectionString ?? throw new ArgumentNullException(nameof(ConnectionString), "Missing key field"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
            modelBuilder.ApplyConfiguration(new RoleConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
            modelBuilder.ApplyConfiguration(new UserLoginConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
            modelBuilder.ApplyConfiguration(new RoleClaimConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
            modelBuilder.ApplyConfiguration(new UserTokenConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
            modelBuilder.ApplyConfiguration(new PasswordHistoryConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
            modelBuilder.ApplyConfiguration(new CaptchaConfiguration() { Schema = IdentityConstants.SECURITY_SCHEMA, SeedData = SeedDataCommon });
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            bool hasChangesBeforeSave = ChangeTracker.HasChanges();
            int saveChangesCount = await base.SaveChangesAsync(cancellationToken: cancellationToken);
            return hasChangesBeforeSave == false || saveChangesCount > 0;
        }
    }
}
