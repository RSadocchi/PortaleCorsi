using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public partial class UserClaim : Microsoft.AspNetCore.Identity.IdentityUserClaim<int>, IIdentityEntity
    { }
}