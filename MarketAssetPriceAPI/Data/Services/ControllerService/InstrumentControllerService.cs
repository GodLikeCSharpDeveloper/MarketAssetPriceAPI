using MarketAssetPriceAPI.Data.Models.ApiProviderModels.ConnectionModels;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Exchanges;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;
using MarketAssetPriceAPI.Data.Services.DbService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public class InstrumentControllerService(IInstrumentService instrumentService, HttpClient httpClient, ITokenControllerService tokenService, IOptions<FintachartCredentials> credentials) : AuthorizedControllerService(tokenService), IInstrumentControllerService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly FintachartCredentials _credentials = credentials.Value;
        private readonly IInstrumentService instrumentService = instrumentService;

        public async Task<InstrumentsResponse> GetInstruments(InstrumentQueryParameters parameters)
        {
            try
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
                await instrumentService.AddOrUpdateNewInstruments(deserializedResponse.Instruments);
                return deserializedResponse;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private string ConstructInstrumentsUrl(InstrumentQueryParameters parameters)
        {
            var uriBuilder = new UriBuilder(_credentials.BaseUrl);
            string endpoint = "api/instruments/v1/instruments";
            uriBuilder.Path += endpoint;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (!string.IsNullOrEmpty(parameters.Provider))
                query["provider"] += parameters.Provider;
            if (!string.IsNullOrEmpty(parameters.Kind))
                query["kind"] += parameters.Kind;
            if (!string.IsNullOrEmpty(parameters.Symbol))
                query["symbol"] += parameters.Symbol;
            if (parameters.Page != 0)
                query["page"] += parameters.Page;
            if (parameters.Size != 0)
                query["size"] += parameters.Size;
            uriBuilder.Query = query.ToString();
            var resultString = uriBuilder.ToString();
            return resultString;
        }
        public async Task<ExchangeResponse> GetExchanges(ExchangesQueryParameters parameters)
        {
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }
        private string ConstructExchangesUrl(ExchangesQueryParameters parameters)
        {
            var uriBuilder = new UriBuilder(_credentials.BaseUrl);
            string endpoint = "api/instruments/v1/exchanges";
            uriBuilder.Path += endpoint;
            if (!string.IsNullOrEmpty(parameters.Provider))
                uriBuilder.Query = $"provider={parameters.Provider}";
            var result = uriBuilder.ToString();
            return result;
        }
        public async Task<Providers> GetProviders()
        {
            try
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
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
