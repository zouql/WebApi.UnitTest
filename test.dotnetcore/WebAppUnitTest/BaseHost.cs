namespace WebAppUnitTest
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using WebApi.UnitTest.DotNetCore;
    using WebApp.Accounts;

    public class BaseHost : BaseTestHost
    {
        private static readonly string UserName = "Admin";

        private static readonly string PassWord = "Aa_123456";
        
        /// <summary>
        /// 环境变量名
        /// </summary>
        protected override string EnvironmentName => "Development";

        /// <summary>
        /// 启动项目名
        /// </summary>
        protected override string StartProjectName => nameof(WebApp);

        public BaseHost()
        {
            StartHost<WebApp.Startup>(m =>
            {
                m.GetRequiredService<AccountService>()?.CreateOrUpdateAsync(UserName, PassWord, new string[0]).GetAwaiter().GetResult();
            });
        }

        /// <summary>
        /// 登陆（Cookie）
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        protected async Task SignInForCookieAsync(string userName = "Admin", string passWord = "Aa_123456")
        {
            var url = $"api/Account/SignInForIdentityAsync?userName={userName}&passWord={passWord}";

            var response = await GetAsync(url);

            AddOrUpdateCookies(response);
        }
    }
}
