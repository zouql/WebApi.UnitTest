namespace WebAppUnitTest
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using WebApi.UnitTest.DotNetCore;
    using WebApp.Accounts;

    public class BaseHost : BaseTestHost
    {
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
            StartHost<WebApp.Startup>();
        }

        /// <summary>
        /// 登陆（Cookie）
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        protected async Task SignInForCookieAsync(string userName = "Admin", string passWord = "Aa_123456")
        {
            await CreateOrUpdateAsync(userName, passWord);

            var url = $"api/Account/SignInForIdentityAsync?userName={userName}&passWord={passWord}";

            var response = await GetAsync(url);

            AddOrUpdateCookies(response);
        }

        /// <summary>
        /// 登陆（Cookie）
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        private async Task CreateOrUpdateAsync(string userName, string passWord)
        {
            var url = $"api/Account/CreateOrUpdateAsync?userName={userName}&passWord={passWord}";

            var response = await GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception(response.StatusCode.ToString());
            }
        }
    }
}
