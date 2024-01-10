using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestIdentity
{
    public partial class UserToken : Microsoft.AspNetCore.Identity.IdentityUserToken<int>, IIdentityEntity
    {
        public bool IsBiometricAuth { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BiometricAuthIssueDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BiometricAuthExpireDateTime { get; set; }
        [MaxLength(128)]
        public string? BiometricIDCode { get; set; }

        public virtual User User { get; set; }
    }
}