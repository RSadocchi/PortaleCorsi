namespace TestIdentity
{
    public class Const_TwoFactorProviders
    {
        public static string Phone => nameof(Phone);
        public static string Email => nameof(Email);
        public static string Authenticator => nameof(Authenticator);
        public static string AuthenticatorUriFormat => "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
    }

}
