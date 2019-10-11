namespace WebApp30.Accounts.AppRoles
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;

    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(
            IRoleStore<AppRole> store,
            IEnumerable<IRoleValidator<AppRole>> roleValidators = null,
            ILookupNormalizer keyNormalizer = null,
            IdentityErrorDescriber errors = null,
            ILogger<RoleManager<AppRole>> logger = null)
            : base(
                 store,
                 roleValidators,
                 keyNormalizer,
                 errors,
                 logger)
        {
        }
    }
}
