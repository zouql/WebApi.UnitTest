namespace WebApp30.Accounts.AppUsers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// 登陆管理类
    /// </summary>
    public class AppSignInManager : SignInManager<AppUser>
    {
        public AppSignInManager(
            UserManager<AppUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<AppUser> claimsFactory = default,
            IOptions<IdentityOptions> optionsAccessor = default,
            ILogger<SignInManager<AppUser>> logger = default,
            IAuthenticationSchemeProvider schemes = default,
            IUserConfirmation<AppUser> confirmation = default)
            : base(
                 userManager,
                 contextAccessor,
                 claimsFactory,
                 optionsAccessor,
                 logger,
                 schemes,
                 confirmation)
        {
        }

        public override Task SignInAsync(AppUser user, bool isPersistent = true, string authenticationMethod = default)
        {
            return base.SignInAsync(user, isPersistent, authenticationMethod);
        }

        public override Task SignInAsync(AppUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = default)
        {
            return base.SignInAsync(user, authenticationProperties, authenticationMethod);
        }
    }
}
