namespace WebApp30.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using WebApp30.Accounts.AppRoles;
    using WebApp30.Accounts.AppUsers;

    public class AccountService
    {
        public AccountService(
            AppUserManager userMananger,
            AppRoleManager roleMananger,
            AppSignInManager signInManager)
        {
            UserMananger = userMananger;
            RoleManager = roleMananger;
            SignInManager = signInManager;
        }

        public AppUserManager UserMananger { get; }

        public AppRoleManager RoleManager { get; }

        public AppSignInManager SignInManager { get; }

        public async Task<IdentityResult> CreateOrUpdateAsync(string userName, string passWord, IEnumerable<string> roles)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return null;
            }

            var user = await UserMananger.FindByNameAsync(userName) ?? new AppUser { UserName = userName };

            var result = await UserMananger.CreateOrUpdateAsync(user);

            if (result?.Succeeded == true)
            {
                user = await UserMananger.FindByNameAsync(user.UserName);

                result = await UserMananger.RemovePasswordAsync(user);

                result = await UserMananger.AddPasswordAsync(user, passWord);

                result = await UserMananger.SetLockoutEnabledAsync(user, false);

                // 添加角色
                var userRoleNames = await UserMananger.GetRolesAsync(user);

                result = await UserMananger.RemoveFromRolesAsync(user, userRoleNames);

                result = await UserMananger.AddToRolesAsync(user, roles ?? new List<string> { AppRole.NameEnum.管理员.ToString() });
            }

            return result;
        }

        public async Task<bool> SignInForIdentityAsync(string userName, string passWord)
        {
            userName = userName?.Trim() ?? string.Empty;

            var user = await UserMananger.FindByNameAsync(userName);

            // 用户密码检查
            var isChecked = await UserMananger.CheckPasswordAsync(user, passWord);

            if (!isChecked)
            {
                return false;
            }

            await SignInManager.SignOutAsync();

            await SignInManager.SignInAsync(
                user,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
                });

            return true;
        }

        public Task SignOutForIdentityAsync()
        {
            return SignInManager.SignOutAsync();
        }
    }
}
