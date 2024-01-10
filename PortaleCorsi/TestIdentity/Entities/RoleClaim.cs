using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public partial class RoleClaim : Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>
    {
        ///// <summary>
        ///// Rappresenta i valori consentiti nel campo RoleClaim.ClaimValue e UserClaim.ClaimValue
        ///// </summary>
        //public enum RoleClaimValues
        //{
        //    [Display(Name = "Deny", Description = "Permission denied")] DENY = 0,
        //    [Display(Name = "Read", Description = "Reading allowed")] READ = 1,
        //    [Display(Name = "Write", Description = "Writing allowed")] WRITE = 2
        //}
    }
}