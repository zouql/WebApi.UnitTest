namespace WebApi.UnitTest.DotNetCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// 测试基类
    /// </summary>
    public abstract class BaseTestHost : IDisposable
    {
        /// <summary>
        /// Json媒体类
        /// </summary>
        protected readonly string JsonmMediaType = "application/json";

        /// <summary>
        /// HttpClint实例
        /// </summary>
        protected HttpClient Client { get; private set; }

        /// <summary>
        /// 环境变量名
        /// </summary>
        protected abstract string EnvironmentName { get; }

        /// <summary>
        /// 启动项目名
        /// </summary>
        protected abstract string StartProjectName { get; }

        void IDisposable.Dispose()
        {
            Client?.Dispose();
        }

        /// <summary>
        /// 启动站点
        /// </summary>
        /// <typeparam name="TStartup">启动类</typeparam>
        /// <param name="serviceActiton">启动设置</param>
        protected void StartHost<TStartup>(Action<IServiceProvider> serviceActiton = null) where TStartup : class
        {
            var server = new TestServer(CreateWebHostBuilder<TStartup>());
            
            if (serviceActiton != null)
            {
                using (var scope = server.Host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    serviceActiton(services);
                }
            }

            Client = server.CreateClient();
        }

        /// <summary>
        /// 新增或更新Cookies
        /// </summary>
        /// <param name="response">Http响应</param>
        /// <returns></returns>
        protected bool AddOrUpdateCookies(HttpResponseMessage response)
        {
            response.Headers.TryGetValues("Set-Cookie", out var cookies);

            if (cookies == null || cookies.Count() == 0)
            {
                return false;
            }

            Client.DefaultRequestHeaders.Remove("Cookie");

            Client.DefaultRequestHeaders.Add("Cookie", cookies);

            return true;
        }

        /// <summary>
        /// 新增或更新Cookies
        /// </summary>
        /// <param name="cookies">Cookies</param>
        /// <returns></returns>
        protected bool AddOrUpdateCookies(IEnumerable<string> cookies)
        {
            if (cookies == null || cookies.Count() == 0)
            {
                return false;
            }

            Client.DefaultRequestHeaders.Remove("Cookie");

            Client.DefaultRequestHeaders.Add("Cookie", cookies);

            return true;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="requestUri">请求Url</param>
        /// <param name="requestParams">Url参数</param>
        /// <param name="requestHeaders">Header参数</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> GetAsync(
            string requestUri,
            IDictionary<string, string> requestParams = null,
            IDictionary<string, string> requestHeaders = null)
        {
            if (requestParams != null && requestHeaders.Count > 0)
            {
                requestUri = $"{requestUri}?{string.Join("&", requestParams.Select(m => $"{m.Key}={m.Value}"))}";
            }

            foreach (var item in requestHeaders ?? new Dictionary<string, string>())
            {
                Client.DefaultRequestHeaders.Remove(item.Key);
            }

            var response = await Client.GetAsync(requestUri);

            foreach (var item in requestHeaders ?? new Dictionary<string, string>())
            {
                Client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            return response;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="requestUri">请求Url</param>
        /// <param name="requestParams">Body参数</param>
        /// <param name="requestHeaders">Header参数</param>
        /// <param name="mediaType">媒体类型(默认为Json格式)</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PostAsync(
            string requestUri,
            JObject requestParams = null,
            IDictionary<string, string> requestHeaders = null,
            string mediaType = "application/json")
        {
            foreach (var item in requestHeaders ?? new Dictionary<string, string>())
            {
                Client.DefaultRequestHeaders.Remove(item.Key);
            }

            var content = new StringContent(
                requestParams?.ToString(),
                encoding: Encoding.UTF8,
                mediaType: JsonmMediaType);

            var response = await Client.PostAsync(requestUri, content);

            foreach (var item in requestHeaders ?? new Dictionary<string, string>())
            {
                Client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            return response;
        }

        private IWebHostBuilder CreateWebHostBuilder<TStartup>() where TStartup : class
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentName);

            Environment.CurrentDirectory = GetProjectPath(StartProjectName);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("apphosting.json", optional: true)
                .AddJsonFile($"apphosting.{EnvironmentName}.json", optional: true);

            var host = WebHost.CreateDefaultBuilder()
                .UseConfiguration(configuration.Build())
                .UseContentRoot(Environment.CurrentDirectory)
                .UseStartup<TStartup>();

            return host;
        }

        private static string GetProjectPath(string csprojName)
        {
            var applicationBasePath = AppContext.BaseDirectory;

            if (string.IsNullOrEmpty(csprojName))
            {
                return applicationBasePath;
            }

            var directoryInfo = new DirectoryInfo(applicationBasePath);

            var files = Directory.GetFiles(directoryInfo.FullName, "*.csproj", SearchOption.AllDirectories);

            while (files.Count(m => m.EndsWith($"{csprojName}.csproj")) == 0)
            {
                directoryInfo = directoryInfo.Parent;

                files = Directory.GetFiles(directoryInfo.FullName, "*.csproj", SearchOption.AllDirectories);
            }

            return Path.GetDirectoryName(files.FirstOrDefault(m => m.EndsWith($"{csprojName}.csproj")));
        }
    }
}
