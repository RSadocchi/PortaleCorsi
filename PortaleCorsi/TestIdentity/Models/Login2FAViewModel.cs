using System.ComponentModel.DataAnnotations;

namespace TestIdentity
{
    public class Login2FAViewModel
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string TwoFactorCode { get; set; }
        public string Culture { get; set; }
        public bool RememberMe { get; set; }
        public AuthApp2FAQRCodeViewModel QRCodeModel { get; set; }
    }
}
