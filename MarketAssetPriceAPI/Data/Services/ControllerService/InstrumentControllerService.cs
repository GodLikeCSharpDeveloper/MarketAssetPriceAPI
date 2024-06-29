using MarketAssetPriceAPI.Data.Models.ConnectionModels;
using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Models.Exchanges;
using MarketAssetPriceAPI.Data.Models.Instruments;
using MarketAssetPriceAPI.Data.Models.Providers;
using MarketAssetPriceAPI.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace MarketAssetPriceAPI.Data.Services
{
    public class InstrumentControllerService(InstrumentRepository instrumentRepository, HttpClient httpClient, TokenControllerService tokenService, IOptions<FintachartCredentials> credentials) : AuthorizedControllerService(tokenService)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly FintachartCredentials _credentials = credentials.Value;
        private readonly InstrumentRepository instrumentRepository = instrumentRepository;

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
            var instrument = deserializedResponse.Instruments.FirstOrDefault();
            await instrumentRepository.AddNewInstrument(new Models.DTOs.InstrumentEntity
            {
                ApiProviderId = instrument.ApiProviderId,
                BaseCurrency = instrument.BaseCurrency,
                Currency = instrument.Currency,
                Description = instrument.Description,
                Kind = instrument.Kind,
                Symbol = instrument.Symbol,
                TickSize = instrument.TickSize
            });

            return deserializedResponse;
        }
        private string ConstructInstrumentsUrl(InstrumentQueryParameters parameters)
        {
            var uriBuilder = new UriBuilder(_credentials.BaseUrl);
            string endpoint = "api/instruments/v1/instruments?";
            uriBuilder.Path += endpoint;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (!string.IsNullOrEmpty(parameters.Provider))
                query["provider"] += parameters.Provider;
            if (!string.IsNullOrEmpty(parameters.Kind))
                query["kind"] +=parameters.Kind));
            if (!string.IsNullOrEmpty(parameters.Symbol))
                query["kind"] += symbol ={Uri.EscapeDataString(parameters.Symbol)}&");
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
