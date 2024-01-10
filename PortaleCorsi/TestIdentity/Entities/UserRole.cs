using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public partial class UserRole : Microsoft.AspNetCore.Identity.IdentityUserRole<int>, IIdentityEntity
    { }
}