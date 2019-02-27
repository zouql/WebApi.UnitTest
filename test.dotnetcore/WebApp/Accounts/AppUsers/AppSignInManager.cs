namespace WebApp.Accounts.AppUsers
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
            IUserClaimsPrincipalFactory<AppUser> claimsFactory = null,
            IOptions<IdentityOptions> optionsAccessor = null,
            ILogger<SignInManager<AppUser>> logger = null,
            IAuthenticationSchemeProvider schemes = null)
            : base(
                 userManager,
                 contextAccessor,
                 claimsFactory,
                 optionsAccessor,
                 logger,
                 schemes
                 )
        {
        }

        public override Task SignInAsync(AppUser user, bool isPersistent = true, string authenticationMethod = null)
        {
            return base.SignInAsync(user, isPersistent, authenticationMethod);
        }

        public override Task SignInAsync(AppUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            return base.SignInAsync(user, authenticationProperties, authenticationMethod);
        }
    }
}
