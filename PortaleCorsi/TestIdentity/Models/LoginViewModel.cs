using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Culture { get; set; }
        public bool RememberMe { get; set; }
        public string RememberValue { get; set; }
    }
}
