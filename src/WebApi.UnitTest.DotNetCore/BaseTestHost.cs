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

    public abstract class BaseTestHost : IDisposable
    {
        protected readonly string JsonmMediaType = "application/json";

        private static HttpClient HostClient;

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
            HostClient?.Dispose();
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

            HostClient = server.CreateClient();
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

            HostClient.DefaultRequestHeaders.Remove("Cookie");

            HostClient.DefaultRequestHeaders.Add("Cookie", cookies);

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

            HostClient.DefaultRequestHeaders.Remove("Cookie");

            HostClient.DefaultRequestHeaders.Add("Cookie", cookies);

            return true;
        }

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
                HostClient.DefaultRequestHeaders.Remove(item.Key);
            }

            var response = await HostClient.GetAsync(requestUri);

            foreach (var item in requestHeaders ?? new Dictionary<string, string>())
            {
                HostClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            return response;
        }

        protected async Task<HttpResponseMessage> PostAsync(
            string requestUri,
            JObject requestParams = null,
            IDictionary<string, string> requestHeaders = null,
            string mediaType = "application/json")
        {
            foreach (var item in requestHeaders ?? new Dictionary<string, string>())
            {
                HostClient.DefaultRequestHeaders.Remove(item.Key);
            }

            var content = new StringContent(
                requestParams?.ToString(),
                encoding: Encoding.UTF8,
                mediaType: JsonmMediaType);

            var response = await HostClient.PostAsync(requestUri, content);

            foreach (var item in requestHeaders ?? new Dictionary<string, string>())
            {
                HostClient.DefaultRequestHeaders.Add(item.Key, item.Value);
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
