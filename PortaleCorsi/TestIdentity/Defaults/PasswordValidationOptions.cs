using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIdentity
{
    public class PasswordValidationOptions : PasswordOptions
    {
        public int HistoryLimit { get; set; } = 5;
        public int ExpireAfter { get; set; } = 180;
    }
}
