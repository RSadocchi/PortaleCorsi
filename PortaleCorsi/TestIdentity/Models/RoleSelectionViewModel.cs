using System;
using System.Collections.Generic;
using System.Text;

namespace TestIdentity
{
    public class RoleSelectionViewModel
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string SelectedRoleName { get; set; }
        public int? SelectedRolaAnagID { get; set; }
        public string RememberMeValue { get; set; }
    }
}
