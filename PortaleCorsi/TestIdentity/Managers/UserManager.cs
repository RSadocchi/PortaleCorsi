using TestIdentity.SeedWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestIdentity
{
    public class UserManager<TDbContext, TUser> : Microsoft.AspNetCore.Identity.UserManager<TUser>
        where TDbContext : DbContext, IUnitOfWork, IIdentityDbContext
        where TUser : User
    {
        readonly RoleManager _roleManager;
        readonly IHttpContextAccessor _httpContext;
        readonly TDbContext _context;
        readonly PasswordValidationOptions _passwordOptions;
        //readonly MultilevelPasswordValidationItem[]? _multilevelPasswordValidationOptions;

        public static string GenericTokenProvider = "GenericProvider";

        public UserManager(
            RoleManager roleManager,
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger,
            IHttpContextAccessor httpContext,
            TDbContext context,
            IOptions<PasswordValidationOptions> passwordValidationOptions,
            IOptions<MultilevelPasswordValidationItem[]> multilevelPasswordValidationOptions) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _passwordOptions = passwordValidationOptions.Value ?? throw new ArgumentNullException(nameof(passwordValidationOptions));
            //_multilevelPasswordValidationOptions = multilevelPasswordValidationOptions?.Value;
        }

        public ClaimsPrincipal? User => _httpContext.HttpContext?.User;
        public int UserID
        {
            get
            {
                var claims = User?.FindAll(ClaimTypes.NameIdentifier);
                if (claims?.Count() > 0)
                    foreach (var c in claims)
                        if (int.TryParse(c?.Value, out int ID)) return ID;
                return -1;
            }
        }

        public async Task<bool> SetCultureAsync(TUser user, string culture)
        {
            user.Culture = culture;
            var result = await UpdateAsync(user);
            return (result.Succeeded);
        }

        public async Task<TUser?> FindByPhoneNumberAsync(string phoneNumber)
            => await Task.FromResult(Users.FirstOrDefault(t => t.PhoneNumber == phoneNumber));

        public async Task<bool> IsPasswordExpiredAsync(TUser user)
            => await Task.FromResult(user.PasswordExpirationDateTime <= DateTime.Now);

        public async Task<bool> CheckPINAsync(TUser user, string pin)
            => await Task.FromResult(PasswordHasher.VerifyHashedPassword(user, user.PINHash, pin) == PasswordVerificationResult.Success);

        public async Task<IList<Claim>> GetUserClaimsAsync(TUser user, string? roleName = null, bool getUserClaims = true, bool getRoleClaims = true)
        {
            var result = new List<Claim>();
            // Aggiungo i CLAIM dell'utente
            if (getUserClaims)
            {
                var userClaims = await base.GetClaimsAsync(user);
                foreach (var item in userClaims)
                    if (result.Find(t => t.Type == item.Type) == null)
                        result.Add(item);
            }

            // Aggiungo i CLAIM dei ruoli dell'utente
            if (getRoleClaims && SupportsUserRole)
            {
                var roles = await base.GetRolesAsync(user);
                foreach (var rName in roles)
                {
                    if (string.IsNullOrWhiteSpace(roleName) || roleName.Trim().Equals(rName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.Add(new Claim("UserClaimsRole", rName));
                        if (_roleManager.SupportsRoleClaims)
                        {
                            var role = await _roleManager.FindByNameAsync(rName);
                            if (role != null)
                            {
                                var roleClaims = await _roleManager.GetClaimsAsync(role);
                                foreach (var roleClaim in roleClaims)
                                    if (result.Find(t => t.Type == roleClaim.Type) == null)
                                        result.Add(roleClaim);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public async Task<IList<Claim>?> GetClaimsByRoleId(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null) return await _roleManager.GetClaimsAsync(role);
            return null;
        }

        public IdentityResult UserHasRole(TUser user, string role)
        {
            if (user is null || string.IsNullOrWhiteSpace(role))
                return IdentityResult.Failed(new IdentityError() { Code = "M001", Description = "Invalid input params" });

            var match = _context.Set<UserRole>()
                .Where(x => x.UserId == user.Id)
                .Join(
                    inner: _context.Set<Role>(),
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => new { userRole = ur, role = r })
                .Any(x => !string.IsNullOrWhiteSpace(x.role.NormalizedName) && x.role.NormalizedName.Equals(role.ToUpper().Trim()));
            if (match)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(new IdentityError() { Code = "R001", Description = "Missing role" });
        }

        public async Task<IdentityResult> CheckAndValidatePasswordAsync(TUser user, string password)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));


            //if (_multilevelPasswordValidationOptions?.Any() == true)
            //{

            //}

            var spacesRegex = new Regex(@"[\s]");

            if (string.IsNullOrWhiteSpace(password))
                return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_EMPTY, Description = $"Empty value is not allowed" });

            if (spacesRegex.IsMatch(password))
                return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_SPACES, Description = $"Spaces are not allowed" });

            if (password.Length < _passwordOptions.RequiredLength)
                return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_LENGTH, Description = $"Mininum required length is {_passwordOptions.RequiredLength}" });

            if (_passwordOptions.RequireUppercase)
            {
                var regex = new Regex(@"[A-Z]");
                if (!regex.IsMatch(password))
                    return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_MISSING_UPPERCASE, Description = $"At least a uppercase letter is required" });
            }

            if (_passwordOptions.RequireLowercase)
            {
                var regex = new Regex(@"[a-z]");
                if (!regex.IsMatch(password))
                    return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_MISSING_LOWERCASE, Description = $"At least a lowercase letter is required" });
            }

            if (_passwordOptions.RequireDigit)
            {
                var regex = new Regex(@"[\d]");
                if (!regex.IsMatch(password))
                    return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_MISSING_DIGIT, Description = $"At least a number is required" });
            }

            if (_passwordOptions.RequireNonAlphanumeric)
            {
                var regex = new Regex(@"[\W]");
                if (!regex.IsMatch(password))
                    return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_MISSING_NON_ALPHANUMERIC, Description = $"At least a special char is required" });
            }

            if (user.Id > 0 && _passwordOptions.HistoryLimit > 0)
            {
                await _context.PasswordHistory.Where(t => t.UserId == user.Id).LoadAsync();

                if (user.PasswordHistories?.Count > 0)
                {
                    var match = user.PasswordHistories
                        .OrderByDescending(p => p.ChangedDateTime)
                        .Take(_passwordOptions.HistoryLimit)
                        .Select(p => p.PasswordHash)
                        .Any(hash =>
                        {
                            var res = PasswordHasher.VerifyHashedPassword(user, hash, password);
                            return res == PasswordVerificationResult.Success;
                        });

                    if (match)
                        return IdentityResult.Failed(new IdentityError() { Code = Const_Errors.PASSWORD_HISTORY, Description = $"The password has already been used." });
                }
            }

            return await Task.FromResult(IdentityResult.Success);
        }
    }
}
