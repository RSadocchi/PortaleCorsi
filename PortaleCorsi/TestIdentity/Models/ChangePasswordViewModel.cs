using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public abstract class ChangePasswordViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class PasswordExpiredModel : ChangePasswordViewModel
    {
        public string Username { get; set; }
    }
}