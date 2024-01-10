using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIdentity
{
    public class LoginResponse
    {
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? TokenPurpose { get; set; }
        public string? RememberMeValue { get; set; }
        public List<LoginRoleSelectionModel> SelectionOptions { get; set; } = new List<LoginRoleSelectionModel>();
    }
}
