using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestIdentity
{
    public partial class User : Microsoft.AspNetCore.Identity.IdentityUser<int>
    {
        public bool Disabled { get; set; }
        public string? PINHash { get; set; }
        [MaxLength(5)]
        public string? Culture { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EmailConfirmedDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PhoneNumberConfirmedDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PasswordExpirationDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PrivacyAcceptanceDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PolicyAcceptanceDateTime { get; set; }
        [MaxLength(100)]
        public string? TwoFactorTokenProviders { get; set; }

        public virtual ICollection<PasswordHistory> PasswordHistories { get; set; } = new HashSet<PasswordHistory>();
        public virtual ICollection<UserToken> UserTokens { get; set; } = new HashSet<UserToken>();

        public User() : base() { }

        public static explicit operator Microsoft.AspNetCore.Identity.IdentityUser(User Value)
        {
            return (Microsoft.AspNetCore.Identity.IdentityUser)Value;
        }
    }
}