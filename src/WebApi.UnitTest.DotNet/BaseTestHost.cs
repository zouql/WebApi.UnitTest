namespace WebApi.UnitTest.DotNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Owin.Hosting;
    using Newtonsoft.Json.Linq;

    public class BaseTestHost : IDisposable
    {
        protected static readonly string JsonmMediaType = "application/json";

        protected HttpClient Client { get; private set; }

        private IDisposable WebAppSite { get; set; }

        /// <summary>
        /// 启动站点
        /// </summary>
        /// <typeparam name="TStartup">启动类</typeparam>
        /// <param name="hostUrl">站点Base Url</param>
        protected void StartHost<TStartup>(string hostUrl = "http://localhost:9000") where TStartup : class
        {
           hostUrl = hostUrl ?? "http://localhost:9000";

            WebAppSite = WebApp.Start<TStartup>(hostUrl);

            var handler = new HttpClientHandler
            {
                UseCookies = true
            };

            Client = new HttpClient(handler)
            {
                BaseAddress = new Uri(hostUrl)
            };

            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
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
            IDictionary<string, string> requestParams = default,
            IDictionary<string, string> requestHeaders = default)
        {
            if (requestParams != default && requestParams.Count > 0)
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
            JObject requestParams = default,
            IDictionary<string, string> requestHeaders = default,
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

        void IDisposable.Dispose()
        {
            WebAppSite?.Dispose();
        }
    }
}

