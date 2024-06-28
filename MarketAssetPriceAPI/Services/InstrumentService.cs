using MarketAssetPriceAPI.Models;
using MarketAssetPriceAPI.Models.Exchanges;
using MarketAssetPriceAPI.Models.Instruments;
using MarketAssetPriceAPI.Models.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MarketAssetPriceAPI.Services
{
    public class FintachartsService(HttpClient httpClient, TokenService tokenService, IOptions<FintachartCredentials> credentials) : AuthorizedService(tokenService)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly FintachartCredentials _credentials = credentials.Value;

        public async Task<InstrumentsResponse> GetInstrumentsAsync(InstrumentQueryParameters parameters)
        {
            await SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync(ConstructInstrumentsUrl(parameters));
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReinitializeAuthorizationAsync(_httpClient);
                response = await _httpClient.GetAsync(ConstructInstrumentsUrl(parameters));
            }
            response.EnsureSuccessStatusCode();
            var deserializedResponse = JsonConvert.DeserializeObject<InstrumentsResponse>(await response.Content.ReadAsStringAsync());
            return deserializedResponse;
        }
        private string ConstructInstrumentsUrl(InstrumentQueryParameters parameters)
        {
            var queryString = new StringBuilder($"{_credentials.BaseUrl}/api/instruments/v1/instruments?");
            if (!string.IsNullOrEmpty(parameters.Provider))
                queryString.Append($"provider={Uri.EscapeDataString(parameters.Provider)}&");
            if (!string.IsNullOrEmpty(parameters.Kind))
                queryString.Append($"kind={Uri.EscapeDataString(parameters.Kind)}&");
            if (!string.IsNullOrEmpty(parameters.Symbol))
                queryString.Append($"symbol={Uri.EscapeDataString(parameters.Symbol)}&");
            if (parameters.Page != 0)
                queryString.Append($"page={parameters.Page}&");
            if (parameters.Size != 0)
                queryString.Append($"size={parameters.Size}&");
            if (queryString[queryString.Length - 1] == '&')
                queryString.Length--;
            return queryString.ToString();
        }
        public async Task<ExchangeResponse> GetExchangesAsync(ExchangesQueryParameters parameters)
        {
            await SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync(ConstructExchangesUrl(parameters));
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReinitializeAuthorizationAsync(_httpClient);
                response = await _httpClient.GetAsync(ConstructExchangesUrl(parameters));
            }
            response.EnsureSuccessStatusCode();
            var deserializedResponse = JsonConvert.DeserializeObject<ExchangeResponse>(await response.Content.ReadAsStringAsync());
            return deserializedResponse;
        }
        private string ConstructExchangesUrl(ExchangesQueryParameters parameters)
        {
            var queryString = new StringBuilder($"{_credentials.BaseUrl}/api/instruments/v1/exchanges");
            if (!string.IsNullOrEmpty(parameters.Provider))
                queryString.Append($"?provider={parameters.Provider}");
            return queryString.ToString();
        }
        public async Task<Providers> GetProvidersAsync()
        {
            await SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync($"{_credentials.BaseUrl}/api/instruments/v1/providers");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReinitializeAuthorizationAsync(_httpClient);
                response = await _httpClient.GetAsync($"{_credentials.BaseUrl}/api/instruments/v1/providers");
            }
            response.EnsureSuccessStatusCode();
            var deserializedResponse = JsonConvert.DeserializeObject<Providers>(await response.Content.ReadAsStringAsync());
            return deserializedResponse;
        }
    }
}
