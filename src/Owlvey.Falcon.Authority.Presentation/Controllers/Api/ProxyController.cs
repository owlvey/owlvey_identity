using Owlvey.Falcon.Authority.Infra.Settings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Owlvey.Falcon.Authority.Presentation.Controllers.Api
{
    [AllowAnonymous]
    [Route("proxy")]
    public class ProxyController : Controller
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly ApiSettings _apiSettings;
        public ProxyController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiSettings.Value;
        }

        [ResponseCache(CacheProfileName = "Default60")]
        [HttpGet("{*resourceUrl}")]
        public async Task<IActionResult> CallGet([FromRoute]string resourceUrl)
        {
            string queryString = string.Empty;
            if (this.Request.QueryString.HasValue)
                queryString = WebUtility.UrlDecode(this.Request.QueryString.Value);

            var response = await this.Get($"/{resourceUrl}{queryString}");

            if (response.IsSuccessStatusCode)
                return this.Ok(await response.Content.ReadAsStringAsync());
            else
                return this.StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPost("{*resourceUrl}")]
        public async Task<IActionResult> CallPost([FromRoute]string resourceUrl, [FromBody]dynamic resource)
        {
            string queryString = string.Empty;
            if (this.Request.QueryString.HasValue)
                queryString = WebUtility.UrlDecode(this.Request.QueryString.Value);

            var response = await this.Post($"/{resourceUrl}{queryString}", resource);

            if (response.IsSuccessStatusCode)
                return this.Ok(await response.Content.ReadAsStringAsync());
            else
                return this.StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPatch("{*resourceUrl}")]
        public async Task<IActionResult> CallPatch([FromRoute]string resourceUrl, [FromBody]dynamic resource)
        {
            string queryString = string.Empty;
            if (this.Request.QueryString.HasValue)
                queryString = WebUtility.UrlDecode(this.Request.QueryString.Value);

            var response = await this.Patch($"/{resourceUrl}{queryString}", resource);

            if (response.IsSuccessStatusCode)
                return this.Ok(await response.Content.ReadAsStringAsync());
            else
                return this.StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPut("{*resourceUrl}")]
        public async Task<IActionResult> CallPut([FromRoute]string resourceUrl, [FromBody]dynamic resource)
        {
            string queryString = string.Empty;
            if (this.Request.QueryString.HasValue)
                queryString = WebUtility.UrlDecode(this.Request.QueryString.Value);

            var response = await this.Put($"/{resourceUrl}{queryString}", resource);

            if (response.IsSuccessStatusCode)
                return this.Ok(await response.Content.ReadAsStringAsync());
            else
                return this.StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpDelete("{*resourceUrl}")]
        public async Task<IActionResult> CallDelete([FromRoute]string resourceUrl)
        {
            string queryString = string.Empty;
            if (this.Request.QueryString.HasValue)
                queryString = WebUtility.UrlDecode(this.Request.QueryString.Value);

            var response = await this.Delete($"/{resourceUrl}{queryString}");

            if (response.IsSuccessStatusCode)
                return this.Ok(await response.Content.ReadAsStringAsync());
            else
                return this.StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        private async Task<HttpResponseMessage> Get(string url)
        {
            return await this.Send(HttpMethod.Get, url);
        }

        private async Task<HttpResponseMessage> Delete(string url)
        {
            return await this.Send(HttpMethod.Delete, url);
        }

        private async Task<HttpResponseMessage> Patch(string url, object resource)
        {
            return await this.Send(HttpMethod.Patch, url, resource);
        }

        private async Task<HttpResponseMessage> Post(string url, object resource)
        {
            return await this.Send(HttpMethod.Post, url, resource);
        }

        private async Task<HttpResponseMessage> Put(string url, object resource)
        {
            return await this.Send(HttpMethod.Put, url, resource);
        }

        private async Task<string> GetToken()
        {
            /*
            var client = _httpClientFactory.CreateClient("RemoteServerFromService");

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"{_apiSettings.Authority}/connect/token",
                ClientId = _apiSettings.AuthWebClientId,
                ClientSecret = _apiSettings.AuthWebClientSecret,
                Scope = _apiSettings.Scope
            });

            return response.AccessToken;
            */

            return string.Empty;
        }

        private async Task<HttpResponseMessage> Send(HttpMethod httpMethod, string url, object resource = null)
        {
            var client = _httpClientFactory.CreateClient("RemoteServerFromService");

            var token = await GetToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpRequestMessage request = new HttpRequestMessage(httpMethod, _apiSettings.Url + url);

            if (resource != null)
                request.Content = new StringContent(JsonConvert.SerializeObject(resource), Encoding.UTF8, "application/json");

            return await client.SendAsync(request);
        }
    }
}
