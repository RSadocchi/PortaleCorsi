using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public partial class UserLogin : Microsoft.AspNetCore.Identity.IdentityUserLogin<int>, IIdentityEntity
    { }
}