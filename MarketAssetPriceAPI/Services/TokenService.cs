using MarketAssetPriceAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime;

namespace MarketAssetPriceAPI.Services
{
    public class TokenService(IOptions<FintachartCredentials> credentials, HttpClient httpClient)
    {
        private readonly FintachartCredentials credentials = credentials.Value;
        private readonly HttpClient httpClient = httpClient;
        private async Task GetTokensAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://platform.fintacharts.com/identity/realms/fintatech/protocol/openid-connect/token");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", "app-cli"),
                new KeyValuePair<string, string>("username", credentials.UserName),
                new KeyValuePair<string, string>("password", credentials.Password)
            });
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<FintachartCredentials>(result);
            credentials.AccessToken = tokenResponse.AccessToken;
            credentials.RefreshToken = tokenResponse.RefreshToken;
        }
        private async Task<bool> RefreshTokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{credentials.BaseUrl}/identity/realms/fintatech/protocol/openid-connect/token");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", credentials.RefreshToken),
                new KeyValuePair<string, string>("client_id", "app-cli"),
            });
            request.Content = content;

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return false;           

            var result = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<FintachartCredentials>(result);
            credentials.AccessToken = tokenResponse.AccessToken;
            credentials.RefreshToken = tokenResponse.RefreshToken;
            return true;

        }
        public async Task ReinitizeAuthorization()
        {
            if (await RefreshTokenAsync())
                return;
            await GetTokensAsync();
        }
    }
}
