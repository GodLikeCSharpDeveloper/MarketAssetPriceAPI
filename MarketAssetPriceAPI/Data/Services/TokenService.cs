using MarketAssetPriceAPI.Data.Extensions.Tokens;
using MarketAssetPriceAPI.Data.Models.ConnectionModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;

namespace MarketAssetPriceAPI.Data.Services
{
    public class TokenService(IHttpClientFactory httpClientFactory,
        IOptions<FintachartCredentials> credentials,
        ILogger<TokenService> logger,
        TokenResponseStore tokenResponse)
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly FintachartCredentials _credentials = credentials.Value;
        private readonly ILogger<TokenService> _logger = logger;
        private readonly TokenResponseStore _tokenResponse = tokenResponse;

        public async Task<string> GetAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(_tokenResponse.RefreshToken) || IsTokenExpired(_tokenResponse.RefreshToken))
                await ReinitializeAuthorizationAsync();
            else if (string.IsNullOrEmpty(_tokenResponse.AccessToken) || IsTokenExpired(_tokenResponse.AccessToken))
                await RefreshTokenAsync();
            return _tokenResponse.AccessToken;
        }

        public async Task ReinitializeAuthorizationAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var request = ConstructRequest(GetAccessTokenHttpRequestContent());
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                response.EnsureSuccessStatusCode();
            await HandleResponseResult(response);
        }

        private bool IsTokenExpired(string accessToken)
        {
            return accessToken.IsTokenExpired();
        }

        private async Task RefreshTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var request = ConstructRequest(GetRefreshTokenHttpRequestContent());
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                response.EnsureSuccessStatusCode();
            await HandleResponseResult(response);
        }

        private async Task HandleResponseResult(HttpResponseMessage httpResponse)
        {
            var result = await httpResponse.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponseStore>(result);
            if (tokenResponse != null)
            {
                _tokenResponse.AccessToken = tokenResponse.AccessToken;
                _tokenResponse.RefreshToken = tokenResponse.RefreshToken;
            }
        }

        private HttpRequestMessage ConstructRequest(FormUrlEncodedContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_credentials.BaseUrl}/identity/realms/fintatech/protocol/openid-connect/token");
            request.Content = content;
            return request;
        }

        private FormUrlEncodedContent GetRefreshTokenHttpRequestContent()
        {
            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", _tokenResponse.RefreshToken),
                new KeyValuePair<string, string>("client_id", "app-cli"),
            });
        }

        private FormUrlEncodedContent GetAccessTokenHttpRequestContent()
        {
            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", "app-cli"),
                new KeyValuePair<string, string>("username", _credentials.UserName),
                new KeyValuePair<string, string>("password", _credentials.Password)
            });
        }

    }
}
