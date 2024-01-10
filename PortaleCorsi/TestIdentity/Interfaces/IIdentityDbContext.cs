using Microsoft.EntityFrameworkCore;

namespace TestIdentity
{
    public interface IIdentityDbContext
    {
        string? ConnectionString { get; set; }
        bool SeedDataCommon { get; set; }
        DbSet<Captcha> Captchas { get; set; }
        DbSet<PasswordHistory> PasswordHistory { get; set; }
    }
}