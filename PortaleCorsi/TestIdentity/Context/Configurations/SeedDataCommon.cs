using Microsoft.AspNetCore.Identity;

namespace TestIdentity
{
    public class SeedDataCommon
    {
        private static User _admin;
        public static User UserAdmin
        {
            get
            {
                if (_admin != null) return _admin;

                string adminName = "admin";
                string adminEmail = "admin@identity.xx";
                var admin = new User
                {
                    Id = 1,
                    Culture = "it",
                    Disabled = false,
                    Email = adminEmail,
                    NormalizedEmail = adminEmail.ToUpper(),
                    EmailConfirmed = true,
                    EmailConfirmedDateTime = new DateTime(2022, 1, 1, 0, 0, 0),
                    UserName = adminName,
                    NormalizedUserName = adminName.ToUpper(),
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    PasswordExpirationDateTime = new DateTime(2050, 12, 31, 23, 50, 59),
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var adminPasswordHash = new PasswordHasher<User>().HashPassword(admin, "$ys4dm1n!");
                admin.PasswordHash = adminPasswordHash;
                _admin = admin;
                return admin;
            }
        }

        private static User _auto;
        public static User UserAuto
        {
            get
            {
                if (_auto != null) return _auto;

                string autoName = "auto";
                string autoEmail = "auto@identity.xx";
                var auto = new User
                {
                    Id = 2,
                    Culture = "it-IT",
                    Disabled = true,
                    Email = autoEmail,
                    NormalizedEmail = autoEmail.ToUpper(),
                    EmailConfirmed = true,
                    EmailConfirmedDateTime = new DateTime(2022, 1, 1, 0, 0, 0),
                    UserName = autoName,
                    NormalizedUserName = autoName.ToUpper(),
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    PasswordExpirationDateTime = new DateTime(2050, 12, 31, 23, 50, 59),
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var autoPasswordHash = new PasswordHasher<User>().HashPassword(auto, Guid.NewGuid().ToString());
                auto.PasswordHash = autoPasswordHash;

                _auto = auto;
                return auto;
            }
        }

        private static User _batch;
        public static User UserBatch
        {
            get
            {
                if (_batch != null) return _batch;

                string batchName = "batch";
                string batchEmail = "batch@identity.xx";
                var batch = new User
                {
                    Id = 3,
                    Culture = "it-IT",
                    Disabled = true,
                    Email = batchEmail,
                    NormalizedEmail = batchEmail.ToUpper(),
                    EmailConfirmed = true,
                    EmailConfirmedDateTime = new DateTime(2022, 1, 1, 0, 0, 0),
                    UserName = batchName,
                    NormalizedUserName = batchName.ToUpper(),
                    LockoutEnabled = false,
                    TwoFactorEnabled = false,
                    PasswordExpirationDateTime = new DateTime(2050, 12, 31, 23, 50, 59),
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var batchPasswordHash = new PasswordHasher<User>().HashPassword(batch, Guid.NewGuid().ToString());
                batch.PasswordHash = batchPasswordHash;

                _batch = batch;
                return batch;
            }
        }

        public static List<(int id, string name, List<string> claims)> Roles = new List<(int id, string name, List<string> claims)>()
        {
            (1, Const_Roles.Auto, new List<string>() { Const_ClaimTypes.AUTO }),
            (2, Const_Roles.Batch, new List<string>() { Const_ClaimTypes.BATCH }),
            (3, Const_Roles.Administrator, new List<string>() { Const_ClaimTypes.ADMINISTRATOR }),
            (4, Const_Roles.Backoffice_supervisor, new List<string>() { Const_ClaimTypes.BACKOFFICE_SUPERVISOR, Const_ClaimTypes.BACKOFFICE }),
            (5, Const_Roles.Backoffice, new List<string>() { Const_ClaimTypes.BACKOFFICE }),
            (6, Const_Roles.Backoffice_readonly, new List<string>() { Const_ClaimTypes.BACKOFFICE_READONLY }),
            (7, Const_Roles.Sales_supervisor, new List<string>() { Const_ClaimTypes.SALES_SUPERVISOR, Const_ClaimTypes.SALES }),
            (8, Const_Roles.Sales, new List<string>() { Const_ClaimTypes.SALES }),
            (9, Const_Roles.Sales_agent, new List<string>() { Const_ClaimTypes.SALES_AGENT }),
            (10, Const_Roles.Frontoffice_supervisor, new List<string>() { Const_ClaimTypes.FRONTOFFICE_SUPERVISOR, Const_ClaimTypes.FRONTOFFICE }),
            (11, Const_Roles.Frontoffice, new List<string>() { Const_ClaimTypes.FRONTOFFICE }),
            (12, Const_Roles.Customer, new List<string>() { Const_ClaimTypes.CUSTOMER }),
            (13, Const_Roles.Customer_dispositive, new List<string>() { Const_ClaimTypes.CUSTOMER_DISPOSITIVE, Const_ClaimTypes.CUSTOMER }),
            (14, Const_Roles.Customer_executive, new List<string>() { Const_ClaimTypes.CUSTOMER_EXECUTIVE, Const_ClaimTypes.CUSTOMER }),
            (15, Const_Roles.Customer_advisory, new List<string>() { Const_ClaimTypes.CUSTOMER_ADVISORY, Const_ClaimTypes.CUSTOMER }),
        };
    }
}
