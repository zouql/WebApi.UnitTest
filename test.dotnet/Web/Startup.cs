[assembly: Microsoft.Owin.OwinStartup(typeof(Web.Startup))]

namespace Web
{
    using System.Web.Http;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888

            var config = new HttpConfiguration();

            WebApiConfig.Register(config);

            // 注册WebApi组件
            app.UseWebApi(config);
        }
    }
}
