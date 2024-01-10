using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TestIdentity
{
    public interface IIdentityService<TUser>
        where TUser : User
    {
        #region ---Users
        Task<IdentityResult> User_CreateAsync(TUser user, string password);
        Task<IdentityResult> User_UpdateAsync(TUser user);
        Task<IdentityResult> User_DeleteAsync(TUser user);

        Task<bool> User_CheckPasswordAsync(TUser user, string password);
        Task<IdentityResult> User_ValidatePasswordAsync(TUser user, string password);
        Task<IdentityResult> User_ChangePasswordAsync(TUser user, string currentPassword, string newPassword);
        Task<string> User_GeneratePasswordResetTokenAsync(TUser user);
        Task<IdentityResult> User_ResetPasswordAsync(TUser user, string token, string newPassword);

        Task<string> User_GenerateEmailConfirmationTokenAsync(TUser user);
        Task<IdentityResult> User_ConfirmEmailAsync(TUser user, string token);
        
        Task<string> User_GenerateChangePhoneNumberTokenAsync(TUser user, string phoneNumber);
        Task<IdentityResult> User_ChangePhoneNumberAsync(TUser user, string phoneNumber, string token);
        Task<string> User_GenerateTwoFactorTokenAsync(TUser user, string provider);
        Task<bool> User_VerifyTwoFactorTokenAsync(TUser user, string provider, string token);
        
        Task<Login2FAViewModel> User_GenerateAuthQRCodeUriAsync(Login2FAViewModel model, TUser user, string applicationName);

        Task<string> User_GenerateUserTokenAsync(TUser user, EnTokenPurpose purpose);
        Task<bool> User_VerifyUserTokenAsync(TUser user, EnTokenPurpose purpose, string token);

        Task<IList<TUser>> User_GetByAsync(int[]? ids = null, string[]? usernames = null, string[]? emails = null, string[]? phones = null, bool? disabled = null);
        Task<TUser?> User_FindByAsync(int? id = null, string? username = null, string? email = null, string? phone = null);
        Task<TUser> User_GetCurrentUserAsync();
        Task<int> User_GetCurrentUserIdAsync();
        Task<int?> User_GetCurrentUserAnagIdAsync();

        Task<IEnumerable<TUser>?> User_GetByClaimsAsync(IEnumerable<Claim> claims, IEnumerable<Claim>? excludeUserWithClaims = null, bool checkValue = false);
        Task<bool> User_CheckClaimsAsync(TUser user, params string[] claims);
        Task<bool> User_CheckClaimsAsync(TUser user, bool checkValue = false, params Claim[] claims);
        Task<bool> User_HasRoleAsync(TUser user, string role);

        Task User_LoginAsync(string username, string password, bool? rememberMe = null, string? culture = null);
        Task User_LogoutAsync();
        Task User_RefreshSignInAsync(TUser user);

        Task<LoginResponse> User_PurposalTokenizer(TUser user, EnTokenPurpose purpose);
        Task<LoginResponse> User_Tokenizer(TUser user, string? culture = null, string? roleName = null, bool getUserClaims = true, bool getRoleClaims = true, params Claim[] claims);
        Task<LoginResponse> User_TokenLogin2FAAsync(string username, string token, string code, string? culture = null);
        Task<LoginResponse> User_TokenLoginAPIAsync(string username, string password, bool rememberMe = false, string? rememberValue = null, string? culture = null);
        Task<LoginResponse> User_TokenLoginPINAsync(string username, string pin, string? culture = null);
        Task<LoginResponse> User_TokenLoginWithRole(TUser user, string? role = null, params Claim[] claims);
        Task User_TokenLogoutAsync();
        #endregion

        #region ---Roles
        Task<IdentityResult> Role_AddAsync(string role, params Claim[] claims);
        Task<IdentityResult> Role_RemoveAsync(string role);

        Task<IdentityResult> Role_AddToAsync(TUser user, params string[] roles);
        Task<IdentityResult> Role_RemoveFromAsync(TUser user, params string[] roles);

        Task<IList<string>> Role_GetByAsync(TUser? user = null, params Claim[] claimRoleIdentifiers);
        Task<IEnumerable<TUser>> Role_GetUsersAsync(params string[] roles);
        #endregion

        #region ---Claims
        Task<IdentityResult> Claims_AddAsync(string role, params Claim[] claims);
        Task<IdentityResult> Claims_UpdateAsync(string role, params Claim[] claims);
        Task<IdentityResult> Claims_RemoveAsync(string role, params Claim[] claims);

        Task<IdentityResult> Claims_AddAsync(TUser user, params Claim[] claims);
        Task<IdentityResult> Claims_UpdateAsync(TUser user, params Claim[] claims);
        Task<IdentityResult> Claims_RemoveAsync(TUser user, params Claim[] claims);

        Task<IList<Claim>?> Claims_GetByAsync(TUser? user = null, string? role = null, bool getUserClaims = true, bool getRoleClaims = true);
        #endregion

        #region ---Utilities
        Task<bool> HasAnyClaimAsync(TUser user, params string[] claims);
        Task<bool> HasAnyClaimAsync(HttpContext httpContext, params string[] claims);
        Task<bool> HasAnyClaimAsync(TUser user, bool matchValue, params Claim[] claims);
        Task<bool> HasAnyClaimAsync(HttpContext httpContext, bool matchValue, params Claim[] claims);
        #endregion
    }
}