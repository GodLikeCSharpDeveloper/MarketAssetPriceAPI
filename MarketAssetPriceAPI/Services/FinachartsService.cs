using MarketAssetPriceAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime;

namespace MarketAssetPriceAPI.Services
{
    public class FintachartsService
    {
        private readonly HttpClient httpClient;
        private readonly FintachartCredentials credentials;
        private readonly TokenService tokenService;
        public FintachartsService(HttpClient httpClient, IOptions<FintachartCredentials> credentials, TokenService tokenService)
        {
            this.httpClient = httpClient;
            this.credentials = credentials.Value;
            this.tokenService = tokenService;
        }

        public async Task<string> GetHistoricalPricesAsync(string asset)
        {
            SetAuthorizationHeaderAsync();
            var response = await httpClient.GetAsync($"{credentials.BaseUrl}/api/instruments/v1/exchanges");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await tokenService.ReinitizeAuthorization();
                response = await httpClient.GetAsync($"{credentials.BaseUrl}/api/instruments/v1/exchanges");
            }
            response.EnsureSuccessStatusCode();
            var responceValue = await response.Content.ReadAsStringAsync();

            return responceValue;
        }
        private void SetAuthorizationHeaderAsync()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.AccessToken);
        }

    }
}
