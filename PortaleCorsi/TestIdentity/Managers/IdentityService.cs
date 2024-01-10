using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using TestIdentity.SeedWork;
using System.Data;

namespace TestIdentity
{
    public class IdentityService<TDbContext, TUser> : IIdentityService<TUser>
        where TDbContext : DbContext, IUnitOfWork, IIdentityDbContext
        where TUser : User
    {
        readonly UserManager<TDbContext, TUser> _userManager;
        readonly RoleManager _roleManager;
        readonly SignInManager<TUser> _signInManager;
        readonly JwtIssuerOptions _jwtOptions;
        readonly UrlEncoder _urlEncoder;
        readonly ILogger<IIdentityService<TUser>> _logger;
        readonly IConfiguration _configuration;
        readonly TDbContext _dbContext;
        readonly PasswordValidationOptions _passwordOptions;

        public IdentityService(
            UserManager<TDbContext, TUser> userManager,
            RoleManager roleManager,
            SignInManager<TUser> signInManager,
            IOptions<JwtIssuerOptions> jwtOptions,
            UrlEncoder urlEncoder,
            ILogger<IIdentityService<TUser>> logger,
            IConfiguration configuration,
            TDbContext dbContext,
            IOptions<PasswordValidationOptions> passwordValidationOptions)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(UserManager<TDbContext, TUser>), "Dependency Injection Error");
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(RoleManager), "Dependency Injection Error");
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(SignInManager<TUser>), "Dependency Injection Error");
            _jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(IOptions<JwtIssuerOptions>), "Dependency Injection Error");
            _urlEncoder = urlEncoder ?? throw new ArgumentNullException(nameof(UrlEncoder), "Dependency Injection Error");
            _logger = logger ?? throw new ArgumentNullException(nameof(IIdentityService<TUser>), "Dependency Injection Error");
            _configuration = configuration ?? throw new ArgumentNullException(nameof(IConfiguration), "Dependency Injection Error");
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Dependency Injection Error");
            _passwordOptions = passwordValidationOptions.Value ?? throw new ArgumentNullException(nameof(passwordValidationOptions), "Dependency Injection Error");
        }

        //id => http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier

        #region --Claims
        public async Task<IdentityResult> Claims_AddAsync(string role, params Claim[] claims)
        {
            try
            {
                if (!(claims?.Length > 0)) throw new ArgumentNullException(nameof(claims), "Missing param");
                var _role = _dbContext.Set<Role>().Where(t => t.Name.Equals(role.Trim(), StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault()
                    ?? throw new ArgumentNullException(role, "Missing role");
                foreach (var _claim in claims)
                {
                    var result = await _roleManager.AddClaimAsync(_role, _claim);
                    if (!result.Succeeded) return result;
                }
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return IdentityResult.Failed(new IdentityError() { Code = "MP001", Description = ex.Message });
            }
        }

        public async Task<IdentityResult> Claims_AddAsync(TUser user, params Claim[] claims)
        {
            try
            {
                if (!(claims?.Length > 0)) throw new ArgumentNullException(nameof(claims), "Missing param");
                if (user is null) throw new ArgumentNullException(nameof(user), "Missing param");
                foreach (var _claim in claims)
                {
                    var result = await _userManager.AddClaimAsync(user, _claim);
                    if (!result.Succeeded) return result;
                }
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return IdentityResult.Failed(new IdentityError() { Code = "MP001", Description = ex.Message });
            }
        }

        public async Task<IList<Claim>?> Claims_GetByAsync(TUser? user = null, string? role = null, bool getUserClaims = true, bool getRoleClaims = true)
        {
            if (user is null && string.IsNullOrWhiteSpace(role)) return null;
            if (user is not null)
                return await _userManager.GetUserClaimsAsync(user: user, roleName: role, getUserClaims: getUserClaims, getRoleClaims: getRoleClaims);
            else if (!string.IsNullOrWhiteSpace(role))
            {
                var _role = _dbContext.Set<Role>().Where(t => role.Trim().Equals(t.Name, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault()
                    ?? throw new ArgumentNullException(role, "Missing role");
                return await _userManager.GetClaimsByRoleId(_role.Id.ToString());
            }
            else
                return null;
        }

        public async Task<IdentityResult> Claims_RemoveAsync(string role, params Claim[] claims)
        {
            if (!(claims?.Length > 0) || string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException(nameof(claims), "Missing param");
            var _role = _dbContext.Set<Role>().Where(t => t.Name.Equals(role.Trim(), StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault()
                ?? throw new ArgumentNullException(role, "Missing role");
            foreach (var _claim in claims)
            {
                var result = await _roleManager.RemoveClaimAsync(role: _role, claim: _claim);
                if (!result.Succeeded) return result;
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> Claims_RemoveAsync(TUser user, params Claim[] claims)
        {
            if (!(claims?.Length > 0) || user is null) throw new ArgumentNullException(nameof(claims), "Missing param");
            foreach (var _c in claims)
            {
                var result = await _userManager.RemoveClaimAsync(user: user, claim: _c);
                if (!result.Succeeded) return result;
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> Claims_UpdateAsync(string role, params Claim[] claims)
        {
            if (!(claims?.Length > 0) || string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException(nameof(claims), "Missing param");
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var _role = _dbContext.Set<Role>().Where(t => t.Name.Equals(role.Trim(), StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault()
                        ?? throw new ArgumentNullException(role, "Missing role");

                    var _claimTypes = claims.Select(t => t.Type).ToArray();
                    var _claimsToRemove = _dbContext.Set<RoleClaim>()
                        .Where(t => t.RoleId == _role.Id)
                        .Where(t => _claimTypes.Contains(t.ClaimType))
                        .Select(t => new Claim(t.ClaimType, t.ClaimValue))
                        .ToArray();

                    foreach (var _c in _claimsToRemove)
                    {
                        var res = await _roleManager.RemoveClaimAsync(role: _role, claim: _c);
                        if (!res.Succeeded) throw new Exception(string.Join(" | ", res.Errors.Select(t => $"[{t.Code}] {t.Description}")));
                    }

                    foreach (var _c in claims)
                    {
                        var res = await _roleManager.AddClaimAsync(role: _role, claim: _c);
                        if (!res.Succeeded) throw new Exception(string.Join(" | ", res.Errors.Select(t => $"[{t.Code}] {t.Description}")));
                    }

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IdentityResult> Claims_UpdateAsync(TUser user, params Claim[] claims)
        {
            if (!(claims?.Length > 0) || user is null) throw new ArgumentNullException(nameof(claims), "Missing param");

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var _claimTypes = claims.Select(t => t.Type).ToArray();
                    var _claimsToRemove = _dbContext.Set<UserClaim>()
                        .Where(t => t.UserId == user.Id)
                        .Where(t => _claimTypes.Contains(t.ClaimType))
                        .Select(t => new Claim(t.ClaimType, t.ClaimValue))
                        .ToArray();

                    foreach (var _c in _claimsToRemove)
                    {
                        var res = await _userManager.RemoveClaimAsync(user: user, claim: _c);
                        if (!res.Succeeded) throw new Exception(string.Join(" | ", res.Errors.Select(t => $"[{t.Code}] {t.Description}")));
                    }

                    foreach (var _c in claims)
                    {
                        var res = await _userManager.AddClaimAsync(user: user, claim: _c);
                        if (!res.Succeeded) throw new Exception(string.Join(" | ", res.Errors.Select(t => $"[{t.Code}] {t.Description}")));
                    }

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion

        #region --Roles
        public async Task<IdentityResult> Role_AddAsync(string role, params Claim[] claims)
        {
            if (!(claims?.Length > 0) || string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException(nameof(claims), "Missing param");
            var _role = _dbContext.Set<Role>().Where(t => t.Name.Equals(role.Trim(), StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
            if (_role is not null) throw new Exception("Role exists");

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _role = new Role() { Name = role, NormalizedName = role.ToUpper() };
                    var res = await _roleManager.CreateAsync(_role);
                    if (!res.Succeeded) throw new Exception(string.Join(" | ", res.Errors.Select(t => $"[{t.Code}] {t.Description}")));

                    foreach (var _c in claims)
                    {
                        res = await _roleManager.AddClaimAsync(role: _role, claim: _c);
                        if (!res.Succeeded) throw new Exception(string.Join(" | ", res.Errors.Select(t => $"[{t.Code}] {t.Description}")));
                    }

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IdentityResult> Role_AddToAsync(TUser user, params string[] roles)
        {
            if (user is null || !(roles?.Length > 0)) throw new Exception("Missing params");
            foreach (var _r in roles)
            {
                var res = await _userManager.AddToRoleAsync(user: user, role: _r);
                if (!res.Succeeded) return res;
            }
            return IdentityResult.Success;
        }

        public async Task<IList<string>> Role_GetByAsync(TUser? user = null, params Claim[] claimRoleIdentifiers)
        {
            if (user is null && !(claimRoleIdentifiers?.Length > 0)) throw new Exception("Missing params");

            var roles = new List<string>();
            if (user is not null)
            {
                var _res = await _userManager.GetRolesAsync(user: user);
                if (_res?.ToArray()?.Length > 0)
                    roles.AddRange(_res.ToArray());
            }

            if (claimRoleIdentifiers?.Length > 0)
            {
                foreach (var cri in claimRoleIdentifiers)
                {
                    var _res = _dbContext.Set<RoleClaim>()
                        .Where(t => t.ClaimType == cri.Type && t.ClaimValue == cri.Value)
                        .FirstOrDefault();
                    if (_res is not null)
                        roles.Add((await _roleManager.FindByIdAsync(_res.RoleId.ToString())).Name);
                }
            }

            return roles.Distinct().ToList();
        }

        public async Task<IEnumerable<TUser>> Role_GetUsersAsync(params string[] roles)
        {
            var users = new List<TUser>();
            if (roles?.Length > 0)
                foreach (var r in roles)
                {
                    var ulist = await _userManager.GetUsersInRoleAsync(roleName: r);
                    if (ulist?.Count > 0)
                        foreach (var u in ulist)
                            if (users.FirstOrDefault(t => t.Id == u.Id) == null)
                                users.Add(u);
                }
            return users;
        }

        public async Task<IdentityResult> Role_RemoveAsync(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentNullException(nameof(role), "Missing param");
            var _role = await _roleManager.FindByNameAsync(roleName: role) ?? throw new Exception("Missing role");
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var userRole2remove = _dbContext.Set<UserRole>()
                        .Where(t => t.RoleId == _role.Id)
                        .ToList();
                    _dbContext.Set<UserRole>().RemoveRange(userRole2remove);

                    var roleClaim2remove = _dbContext.Set<RoleClaim>()
                        .Where(t => t.RoleId == _role.Id)
                        .ToList();
                    _dbContext.Set<RoleClaim>().RemoveRange(roleClaim2remove);

                    await _dbContext.SaveEntitiesAsync();

                    var res = await _roleManager.DeleteAsync(role: _role);
                    if (!res.Succeeded) throw new Exception(string.Join(" | ", res.Errors.Select(t => $"[{t.Code}] {t.Description}")));

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<IdentityResult> Role_RemoveFromAsync(TUser user, params string[] roles)
        {
            if (user is null || !(roles.Length > 0)) throw new Exception("Missing params");
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var r in roles)
                    {
                        var res = await _userManager.RemoveFromRoleAsync(user: user, role: r);
                        if (!res.Succeeded) throw new Exception(string.Join(" | ", res.Errors.Select(t => $"[{t.Code}] {t.Description}")));
                    }

                    await transaction.CommitAsync();
                    return IdentityResult.Success;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion

        #region --Users
        public async Task<IdentityResult> User_ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
        {
            if (user is null || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(currentPassword))
                throw new Exception("Missing params");

            var result = await _userManager.CheckAndValidatePasswordAsync(user: user, password: newPassword);
            if (result.Succeeded)
            {
                result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    if (_passwordOptions.ExpireAfter > 0) user.PasswordExpirationDateTime = DateTime.Today.AddDays(_passwordOptions.ExpireAfter);
                    else user.PasswordExpirationDateTime = null;
                    result = await _userManager.UpdateAsync(user);
                }
            }

            return result;
        }

        public async Task<IdentityResult> User_ChangePhoneNumberAsync(TUser user, string phoneNumber, string token)
        {
            if (user is null || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(token))
                throw new Exception("Missing params");
            return await _userManager.ChangePhoneNumberAsync(user, phoneNumber, token);
        }

        public async Task<bool> User_CheckClaimsAsync(TUser user, params string[] claims)
            => await User_CheckClaimsAsync(user: user, checkValue: false, claims: claims?.Select(t => new Claim(t, Const_ClaimValues.DefaultValueON))?.ToArray() ?? Array.Empty<Claim>());

        public async Task<bool> User_CheckClaimsAsync(TUser user, bool checkValue = false, params Claim[] claims)
        {
            if (user is null || !(claims?.Length > 0)) throw new Exception("Missing params");

            var list = (await Claims_GetByAsync(user: user))?.ToArray() ?? Array.Empty<Claim>();
            if (list?.Length > 0)
            {
                if (checkValue)
                {
                    foreach (var c in claims)
                    {
                        var match = list
                            .FirstOrDefault(t =>
                                t.Type == c.Type &&
                                t.Value
                                    .Split(";", StringSplitOptions.RemoveEmptyEntries)
                                    .OrderBy(tt => tt)
                                    .ToArray()
                                    .Intersect(c.Value
                                        .Split(";", StringSplitOptions.RemoveEmptyEntries)
                                        .OrderBy(tt => tt)
                                        .ToArray())
                                    .Any());
                        if (match is not null) return true;
                    }
                }
                else
                    return list
                        .Select(t => t.Type)
                        .OrderBy(t => t)
                        .ToArray()
                        .Intersect(claims
                            .Select(t => t.Type)
                            .OrderBy(t => t)
                            .ToArray())
                        ?.Count() > 0;
            }

            return false;
        }

        public async Task<bool> User_HasRoleAsync(TUser user, string role)
        {
            var idRes = await Task.FromResult(_userManager.UserHasRole(user: user, role: role));
            return idRes.Succeeded;
        }

        public async Task<bool> User_CheckPasswordAsync(TUser user, string password) => await _userManager.CheckPasswordAsync(user, password);

        public async Task<IdentityResult> User_ValidatePasswordAsync(TUser user, string password) => await _userManager.CheckAndValidatePasswordAsync(user: user, password: password);

        public async Task<IdentityResult> User_ConfirmEmailAsync(TUser user, string token) => await _userManager.ConfirmEmailAsync(user, token);

        public async Task<IdentityResult> User_CreateAsync(TUser user, string password)
        {
            if ((await User_FindByAsync(username: user.UserName, email: user.Email, phone: user.PhoneNumber ?? "noPhoneNumberProvided")) is not null)
                return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.USER_EXISTS, Description = "User already exists" });
            var identityRes = await _userManager.CheckAndValidatePasswordAsync(user: user, password: password);
            if (identityRes?.Succeeded == true)
                return await _userManager.CreateAsync(user, password);
            else
                return identityRes ?? IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_VALIDATION_FAIL, Description = "Password validation fail" });
        }

        public async Task<IdentityResult> User_DeleteAsync(TUser user) => await _userManager.DeleteAsync(user);

        public async Task<TUser?> User_FindByAsync(int? id = null, string? username = null, string? email = null, string? phone = null)
        {
            TUser? user = null;
            if (user is null && id.HasValue) user = await _userManager.FindByIdAsync(id.Value.ToString());
            if (user is null && !string.IsNullOrWhiteSpace(username)) user = await _userManager.FindByNameAsync(username);
            if (user is null && !string.IsNullOrWhiteSpace(email)) user = await _userManager.FindByEmailAsync(email);
            if (user is null && !string.IsNullOrWhiteSpace(phone)) user = await _userManager.FindByPhoneNumberAsync(phone);
            return user;
        }

        public async Task<Login2FAViewModel> User_GenerateAuthQRCodeUriAsync(Login2FAViewModel model, TUser user, string applicationName)
        {
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrWhiteSpace(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            model.QRCodeModel = new AuthApp2FAQRCodeViewModel
            {
                SharedKey = _formatKey(unformattedKey),
                AuthenticatorUri = string.Format(Const_TwoFactorProviders.AuthenticatorUriFormat, _urlEncoder.Encode(applicationName), _urlEncoder.Encode(user.UserName), unformattedKey)
            };

            string _formatKey(string source)
            {
                var result = new StringBuilder();
                int currentPosition = 0;
                while (currentPosition + 4 < source.Length)
                {
                    result.Append(source.AsSpan(currentPosition, 4)).Append(' ');
                    currentPosition += 4;
                }
                if (currentPosition < source.Length)
                    result.Append(source.AsSpan(currentPosition));
                return result.ToString().ToLowerInvariant();
            }
            return model;
        }

        public async Task<string> User_GenerateChangePhoneNumberTokenAsync(TUser user, string phoneNumber)
             => await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);

        public async Task<string> User_GenerateEmailConfirmationTokenAsync(TUser user)
             => await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<string> User_GeneratePasswordResetTokenAsync(TUser user)
             => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<string> User_GenerateTwoFactorTokenAsync(TUser user, string provider)
             => await _userManager.GenerateTwoFactorTokenAsync(user, provider);

        public async Task<string> User_GenerateUserTokenAsync(TUser user, EnTokenPurpose purpose)
             => await _userManager.GenerateUserTokenAsync(user, UserManager<TDbContext, TUser>.GenericTokenProvider, $"{purpose:G}");

        public async Task<IList<TUser>> User_GetByAsync(int[]? ids = null, string[]? usernames = null, string[]? emails = null, string[]? phones = null, bool? disabled = null)
        {
            var users = _userManager.Users;
            if (ids?.Length > 0) users = users.Where(t => ids.Contains(t.Id));
            if (usernames?.Where(t => !string.IsNullOrWhiteSpace(t))?.Count() > 0)
            {
                usernames = usernames.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.ToUpper().Trim()).ToArray();
                users = users.Where(t => !string.IsNullOrWhiteSpace(t.NormalizedUserName) && usernames.Contains(t.NormalizedUserName.Trim()));
            }
            if (emails?.Where(t => !string.IsNullOrWhiteSpace(t))?.Count() > 0)
            {
                emails = emails.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.ToUpper().Trim()).ToArray();
                users = users.Where(t => !string.IsNullOrWhiteSpace(t.NormalizedEmail) && emails.Contains(t.NormalizedEmail.Trim()));
            }
            if (phones?.Where(t => !string.IsNullOrWhiteSpace(t))?.Count() > 0)
            {
                phones = phones.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.ToUpper().Trim()).ToArray();
                users = users.Where(t => !string.IsNullOrWhiteSpace(t.PhoneNumber) && phones.Contains(t.PhoneNumber.Trim()));
            }
            if (disabled.HasValue) users = users.Where(t => t.Disabled == disabled.Value);
            return await Task.FromResult(users.ToList());
        }

        public async Task<IEnumerable<TUser>?> User_GetByClaimsAsync(IEnumerable<Claim> claims, IEnumerable<Claim>? excludeUserWithClaims = null, bool checkValue = false)
        {
            if (claims?.Count() > 0)
            {
                var roles = await _roleManager.GetRolesByClaimTypes(claimTypes: claims.Select(t => t.Type).ToArray());
                var tempUsers = new List<TUser>();

                foreach (var role in (roles?.DistinctBy(t => t.Name) ?? new List<Role>()))
                    tempUsers.AddRange(await _userManager.GetUsersInRoleAsync(role.Name));

                var users = new List<TUser>();
                tempUsers = tempUsers?.DistinctBy(t => t.Id)?.ToList();
                if (tempUsers?.Count > 0)
                    foreach (var usr in tempUsers)
                        foreach (var role in (roles?.DistinctBy(t => t.Name) ?? new List<Role>()))
                        {
                            var userClaims = await Claims_GetByAsync(user: usr, role: role.Name);
                            if (claims.Select(t => t.Type).Intersect(userClaims.Select(t => t.Type))?.Count() > 0 &&
                                (!(excludeUserWithClaims?.Count() > 0) || excludeUserWithClaims.Select(t => t.Type).Intersect(userClaims.Select(t => t.Type))?.Count() <= 0))
                                users.Add(usr);
                        }

                return await Task.FromResult(users?.DistinctBy(t => t.Id));
            }

            return null;
        }

        public async Task<int?> User_GetCurrentUserAnagIdAsync()
        {
            if (int.TryParse(_userManager.User?.FindFirst(Const_ClaimTypes.ANAG_ID.ToString())?.Value ?? "-1", out int anag_id))
                return await Task.FromResult((anag_id > 0) ? (int?)anag_id : null);
            else
                return null;
        }

        public async Task<TUser> User_GetCurrentUserAsync() => await _userManager.FindByIdAsync(_userManager.UserID.ToString());

        public async Task<int> User_GetCurrentUserIdAsync() => await Task.FromResult(_userManager.UserID);

        public async Task User_LoginAsync(string username, string password, bool? rememberMe = null, string? culture = null)
        {
            username = username.Trim();
            password = password.Trim();

            var user = await User_FindByAsync(username: username) ?? throw new Exception(Const_Errors.USER_CREDENTIALS);
            if (await _userManager.IsLockedOutAsync(user)) throw new Exception(Const_Errors.USER_LOCKEDOUT);
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                if (await _userManager.GetLockoutEnabledAsync(user)) await _userManager.AccessFailedAsync(user);
                throw new Exception(Const_Errors.USER_CREDENTIALS);
            }
            else
            {
                if (user.Disabled)
                {
                    if (!user.EmailConfirmed) throw new Exception(Const_Errors.USER_EMAIL_UNCONFIRMED);
                    else throw new Exception(Const_Errors.USER_DISABLED);
                }
                else if (username.Equals(user.PhoneNumber) && !user.PhoneNumberConfirmed)
                    throw new Exception(Const_Errors.USER_PHONENUMBER_UNCONFIRMED);

                await _signInManager.SignInAsync(user: user, isPersistent: rememberMe.GetValueOrDefault(false));
                await Task.CompletedTask;
            }
        }

        public async Task User_LogoutAsync() => await _signInManager.SignOutAsync();

        public async Task<LoginResponse> User_PurposalTokenizer(TUser user, EnTokenPurpose purpose)
        {
            var nextPurpose = EnTokenPurpose.Login;
            if (purpose == EnTokenPurpose.Login && user.TwoFactorEnabled)
                nextPurpose = EnTokenPurpose.TwoFactor;
            else if (await _userManager.IsPasswordExpiredAsync(user))
                nextPurpose = EnTokenPurpose.ChangePassword;
            else
                nextPurpose = EnTokenPurpose.RoleSelection;

            return new LoginResponse()
            {
                Token = (nextPurpose == EnTokenPurpose.ChangePassword) ?
                    WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(await User_GeneratePasswordResetTokenAsync(user))) :
                    await this.User_GenerateUserTokenAsync(user, nextPurpose),
                TokenPurpose = nextPurpose.ToString(),
                RememberMeValue = user.UserName.ToMD5()
            };
        }

        public async Task User_RefreshSignInAsync(TUser user) => await _signInManager.RefreshSignInAsync(user);

        public async Task<IdentityResult> User_ResetPasswordAsync(TUser user, string token, string newPassword)
        {
            var result = await _userManager.CheckAndValidatePasswordAsync(user: user, password: newPassword);
            if (result.Succeeded)
            {
                result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (result.Succeeded)
                {
                    if (_passwordOptions.ExpireAfter > 0) user.PasswordExpirationDateTime = DateTime.Today.AddDays(_passwordOptions.ExpireAfter);
                    else user.PasswordExpirationDateTime = null;
                    result = await User_UpdateAsync(user);
                }
            }
            return result;
        }

        public async Task<LoginResponse> User_Tokenizer(TUser user, string? culture = null, string? roleName = null, bool getUserClaims = true, bool getRoleClaims = true, params Claim[] claims)
        {
            if (user is null) throw new ArgumentNullException(nameof(User), "Missing param");

            await _userManager.ResetAccessFailedCountAsync(user);

            if (user.Disabled && user.EmailConfirmed == true) throw new Exception("USER_DISABLED");
            if (user.Disabled && user.EmailConfirmed == false) throw new Exception("USER_EMAIL_UNCONFIRMED");

            if (!string.IsNullOrWhiteSpace(culture) && user?.Culture != culture) await _userManager.SetCultureAsync(user, culture);

            //Genero un token generico di accesso senza ruoli, il ruolo viene scelto successivamente in base ai ruoli definiti nel collegamento Anag_AnagXAnagLink
            IList<Claim> userClaims = new List<Claim>();
            if (claims?.Length > 0)
                foreach (Claim c in claims)
                    userClaims.Add(c);

            foreach (Claim c in await _userManager.GetUserClaimsAsync(user, roleName: roleName, getRoleClaims: getRoleClaims, getUserClaims: getUserClaims))
                if (userClaims.Where(uc => uc.Type == c.Type).Count() == 0)
                    userClaims.Add(c);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, (DateTime.UtcNow).ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, (DateTime.UtcNow).ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            userClaims.Add(new Claim("culture", user.Culture ?? "", ClaimValueTypes.String));

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: userClaims,
                notBefore: (DateTime.UtcNow),
                expires: (DateTime.UtcNow).Add(TimeSpan.FromMinutes(_jwtOptions.ValidFor)),
                signingCredentials: _jwtOptions.SigningCredentials);

            return new LoginResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                TokenPurpose = null
            };
        }

        public async Task<LoginResponse> User_TokenLogin2FAAsync(string username, string token, string code, string? culture = null)
        {
            var user = await User_FindByAsync(username: username) ?? throw new Exception("USER_NOT_FOUND");
            if (!await User_VerifyUserTokenAsync(user, EnTokenPurpose.TwoFactor, token)) throw new Exception("USER_NOT_FOUND");
            if (await _userManager.IsLockedOutAsync(user)) throw new Exception("USER_LOCKEDOUT");
            var twoFactorTokenProvider = user.TwoFactorTokenProviders?.Split(";", StringSplitOptions.RemoveEmptyEntries)?.FirstOrDefault()?.Trim();
            if (await _userManager.VerifyTwoFactorTokenAsync(user, twoFactorTokenProvider, code)) return await User_PurposalTokenizer(user, EnTokenPurpose.TwoFactor);
            if (await _userManager.GetLockoutEnabledAsync(user)) await _userManager.AccessFailedAsync(user);
            throw new Exception("USER_2FA_CODE");
        }

        public async Task<LoginResponse> User_TokenLoginAPIAsync(string username, string password, bool rememberMe = false, string? rememberValue = null, string? culture = null)
        {
            var user = await User_FindByAsync(username: username) ?? throw new Exception("USER_NOT_FOUND");
            if (await _userManager.IsLockedOutAsync(user)) throw new Exception("USER_LOCKEDOUT");
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                if (await _userManager.GetLockoutEnabledAsync(user)) await _userManager.AccessFailedAsync(user);
                throw new Exception("USER_CREDENTIALS");
            }
            else
            {
                if (user.Disabled)
                {
                    if (!user.EmailConfirmed) throw new Exception("USER_EMAIL_UNCONFIRMED");
                    else throw new Exception("USER_DISABLED");
                }
                return await User_PurposalTokenizer(user, EnTokenPurpose.Login);
            }
            throw new Exception("USER_CREDENTIALS");
        }

        public async Task<LoginResponse> User_TokenLoginPINAsync(string username, string pin, string? culture = null)
        {
            var user = await User_FindByAsync(username: username) ?? throw new Exception("USER_NOT_FOUND");
            if (await _userManager.IsLockedOutAsync(user)) throw new Exception("USER_LOCKEDOUT");
            if (await _userManager.CheckPINAsync(user, pin))
            {
                await _userManager.SetCultureAsync(user, culture);
                if (user.TwoFactorEnabled)
                    return await User_PurposalTokenizer(user, EnTokenPurpose.TwoFactor);
                if (await _userManager.IsPasswordExpiredAsync(user))
                    return await User_PurposalTokenizer(user, EnTokenPurpose.ChangePassword);
                else
                    return await User_PurposalTokenizer(user, EnTokenPurpose.RoleSelection);
            }
            if (await _userManager.GetLockoutEnabledAsync(user))
                await _userManager.AccessFailedAsync(user);
            throw new Exception("USER_CREDENTIALS");
        }

        public async Task<LoginResponse> User_TokenLoginWithRole(TUser user, string role, params Claim[] claims)
            => await User_Tokenizer(user, culture: user.Culture, roleName: role, claims: claims);

        public async Task User_TokenLogoutAsync()
        {
            var user = await User_GetCurrentUserAsync();
            await _userManager.RemoveAuthenticationTokenAsync(user, _jwtOptions.Issuer, _jwtOptions.Audience);
        }

        public async Task<IdentityResult> User_UpdateAsync(TUser user)
        {
            if ((await User_FindByAsync(id: user.Id, username: user.UserName, email: user.Email, phone: user.PhoneNumber)) is null)
                return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.USER_MISSING, Description = "Not found" });
            return await _userManager.UpdateAsync(user);
        }

        public async Task<bool> User_VerifyTwoFactorTokenAsync(TUser user, string provider, string token)
             => await _userManager.VerifyTwoFactorTokenAsync(user, provider, token);

        public async Task<bool> User_VerifyUserTokenAsync(TUser user, EnTokenPurpose purpose, string token)
             => await _userManager.VerifyUserTokenAsync(user, UserManager<TDbContext, TUser>.GenericTokenProvider, $"{purpose:G}", token);
        #endregion

        #region ---Utilities
        public async Task<bool> HasAnyClaimAsync(TUser user, params string[] claims)
            => await HasAnyClaimAsync(user: user, matchValue: false, claims: claims?.Select(x => new Claim(x, Const_ClaimValues.DefaultValueON))?.ToArray() ?? Array.Empty<Claim>());

        public async Task<bool> HasAnyClaimAsync(HttpContext httpContext, params string[] claims)
            => await HasAnyClaimAsync(httpContext: httpContext, matchValue: false, claims: claims?.Select(x => new Claim(x, Const_ClaimValues.DefaultValueON))?.ToArray() ?? Array.Empty<Claim>());

        public async Task<bool> HasAnyClaimAsync(TUser user, bool matchValue, params Claim[] claims)
        {
            claims ??= Array.Empty<Claim>();
            if (user is null || claims.Length <= 0) throw new ArgumentNullException();
            var userClaims = (await Claims_GetByAsync(user: user, getUserClaims: true, getRoleClaims: true) ?? Enumerable.Empty<Claim>()).ToList();
            var matches = userClaims
                .Select(x => new { type = x.Type, value = (matchValue ? x.Value : Const_ClaimValues.DefaultValueON) })
                .Intersect(claims.Select(x => new { type = x.Type, value = (matchValue ? x.Value : Const_ClaimValues.DefaultValueON) }))
                .ToArray();
            return matches.Any();
        }

        public async Task<bool> HasAnyClaimAsync(HttpContext httpContext, bool matchValue, params Claim[] claims)
        {
            claims ??= Array.Empty<Claim>();
            if (httpContext is null || claims.Length <= 0) throw new ArgumentNullException();
            if (httpContext.User is null || !httpContext.User.Claims.Any()) return false;
            var matches = httpContext.User.Claims
                .Select(x => new { type = x.Type, value = (matchValue ? x.Value : Const_ClaimValues.DefaultValueON) })
                .Intersect(claims.Select(x => new { type = x.Type, value = (matchValue ? x.Value : Const_ClaimValues.DefaultValueON) }))
                .ToArray();
            return matches.Any();
        }
        #endregion
    }
}
