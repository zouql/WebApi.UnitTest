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
        /// ����������
        /// </summary>
        protected override string EnvironmentName => "Development";

        /// <summary>
        /// ������Ŀ��
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
        /// ��½��Cookie��
        /// </summary>
        /// <param name="userName">�û���</param>
        /// <param name="passWord">����</param>
        /// <returns></returns>
        protected async Task SignInForCookieAsync(string userName = "Admin", string passWord = "Aa_123456")
        {
            var url = $"api/Account/SignInForIdentityAsync?userName={userName}&passWord={passWord}";

            var response = await GetAsync(url);

            AddOrUpdateCookies(response);
        }
    }
}
