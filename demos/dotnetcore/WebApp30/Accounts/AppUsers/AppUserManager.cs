namespace WebApp30.Accounts.AppUsers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(
            IUserStore<AppUser> store,
            IOptions<IdentityOptions> optionsAccessor = null,
            IPasswordHasher<AppUser> passwordHasher = null,
            IEnumerable<IUserValidator<AppUser>> userValidators = null,
            IEnumerable<IPasswordValidator<AppUser>> passwordValidators = null,
            ILookupNormalizer keyNormalizer = null,
            IdentityErrorDescriber errors = null,
            IServiceProvider services = null,
            ILogger<UserManager<AppUser>> logger = null)
            : base(
                  store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
            // 设置密码验证规则
            Options.Password.RequireDigit = true;
            Options.Password.RequiredLength = 6;
            Options.Password.RequireLowercase = true;
            Options.Password.RequireUppercase = false;
            Options.Password.RequireNonAlphanumeric = false;
        }

        public Task<IdentityResult> CreateOrUpdateAsync(AppUser user)
        {
            if (user?.UserName == null)
            {
                return null;
            }

            var isExist = FindByNameAsync(user.UserName).Result != null;

            if (isExist)
            {
                return UpdateAsync(user);
            }

            return CreateAsync(user);
        }
    }
}
