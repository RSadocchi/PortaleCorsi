using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public class LoginPINViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string PIN { get; set; }
        public string Culture { get; set; }
    }
}
