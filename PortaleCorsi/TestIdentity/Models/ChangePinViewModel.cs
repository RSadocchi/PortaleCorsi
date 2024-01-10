using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public abstract class ChangePinViewModel
    {
        [Required]
        public string OldPin { get; set; }
        
        [Required]
        public string NewPin { get; set; }

        [Required]
        public string NewPinConfirm { get; set; }
    }
}