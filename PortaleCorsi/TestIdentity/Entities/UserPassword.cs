using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public partial class PasswordHistory : IIdentityEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime ChangedDateTime { get; set; }

        public User User { get; set; }
    }
}