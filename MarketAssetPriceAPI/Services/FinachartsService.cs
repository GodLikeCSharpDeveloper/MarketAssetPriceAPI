using MarketAssetPriceAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace MarketAssetPriceAPI.Services
{
    public class FintachartsService
    {
        private readonly HttpClient _httpClient;
        private readonly FintachartCredentials _credentials;
        private readonly TokenService _tokenService;

        public FintachartsService(HttpClient httpClient, IOptions<FintachartCredentials> credentials, TokenService tokenService)
        {
            _httpClient = httpClient;
            _credentials = credentials.Value;
            _tokenService = tokenService;
        }

        public async Task<string> GetExchangeDataAsync()
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"{_credentials.BaseUrl}/api/instruments/v1/exchanges");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await ReinitializeAuthorizetionAsync();
                response = await _httpClient.GetAsync($"{_credentials.BaseUrl}/api/instruments/v1/exchanges");
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetInstrumentsAsync(string provider, string kind)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"{_credentials.BaseUrl}/api/instruments/v1/instruments?provider={provider}&kind={kind}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await ReinitializeAuthorizetionAsync();
                response = await _httpClient.GetAsync($"{_credentials.BaseUrl}/api/instruments/v1/instruments?provider={provider}&kind={kind}");
            }
            response.EnsureSuccessStatusCode();
            var instrResponseTest = JsonConvert.DeserializeObject<InstrumentsResponse>(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadAsStringAsync();
        }
        private async Task ReinitializeAuthorizetionAsync()
        {
            await _tokenService.ReinitializeAuthorizationAsync();
            await SetAuthorizationHeaderAsync();
        }
        private async Task SetAuthorizationHeaderAsync()
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
