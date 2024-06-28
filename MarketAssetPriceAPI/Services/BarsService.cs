using MarketAssetPriceAPI.Models;
using MarketAssetPriceAPI.Models.Instruments;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using MarketAssetPriceAPI.Models.Bars;
using MarketAssetPriceAPI.Models.Providers;
using System.Text;
using MarketAssetPriceAPI.Models.Bars.QueryParameters;

namespace MarketAssetPriceAPI.Services
{
    public class BarsService(HttpClient httpClient, IOptions<FintachartCredentials> credentials, TokenService tokenService) : AuthorizedService(tokenService)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly FintachartCredentials _credentials = credentials.Value;
        public async Task<BarsApiResponse> GetBarsCountBackAsync(BarsCountBackQueryParameters queryParams)
        {
            await SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync(GetBarsCountUrl(queryParams));

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReinitializeAuthorizationAsync(_httpClient);
                response = await _httpClient.GetAsync(GetBarsCountUrl(queryParams));
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return await ErrorResponseHandler(response);
            return await SuccessResponseHandler(response);
        }
        private async Task<BarsApiResponse> ErrorResponseHandler(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var responseValue = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
            return new BarsApiResponse()
            {
                IsSuccess = false,
                ErrorResponse = responseValue
            };
        }
        private async Task<BarsApiResponse> SuccessResponseHandler(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            var responseValue = JsonConvert.DeserializeObject<BarsCountResponse>(responseString);
            return new BarsApiResponse()
            {
                IsSuccess = true,
                SuccessResponse = responseValue
            };
        }
        private string GetBarsCountUrl(BarsCountBackQueryParameters parameters)
        {
            var queryString = new StringBuilder($"{_credentials.BaseUrl}/api/bars/v1/bars/count-back?");
            if (!string.IsNullOrEmpty(parameters.InstrumentId))
                queryString.Append($"instrumentId={Uri.EscapeDataString(parameters.InstrumentId)}&");
            if (!string.IsNullOrEmpty(parameters.Provider))
                queryString.Append($"provider={Uri.EscapeDataString(parameters.Provider)}&");
            if (parameters.Interval > 0)
                queryString.Append($"interval={parameters.Interval}&");
            if (!string.IsNullOrEmpty(parameters.Periodicity))
                queryString.Append($"periodicity={Uri.EscapeDataString(parameters.Periodicity)}&");
            if (parameters.BarsCount > 0)
                queryString.Append($"barsCount={parameters.BarsCount}&");
            if (queryString[queryString.Length - 1] == '&')
                queryString.Length--;
            return queryString.ToString();
        }
        public async Task<BarsApiResponse> GetBarsDateRangeAsync(BarsDateRangeQueryParameters queryParams)
        {
            await SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync(GetBarsDateRangeUrl(queryParams));

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReinitializeAuthorizationAsync(_httpClient);
                response = await _httpClient.GetAsync(GetBarsDateRangeUrl(queryParams));
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return await ErrorResponseHandler(response);
            return await SuccessResponseHandler(response);
        }
        private string GetBarsDateRangeUrl(BarsDateRangeQueryParameters parameters)
        {
            var queryString = new StringBuilder($"{_credentials.BaseUrl}/api/bars/v1/bars/date-range?");
            if (!string.IsNullOrEmpty(parameters.InstrumentId))
                queryString.Append($"instrumentId={Uri.EscapeDataString(parameters.InstrumentId)}&");
            if (!string.IsNullOrEmpty(parameters.Provider))
                queryString.Append($"provider={Uri.EscapeDataString(parameters.Provider)}&");
            if (parameters.Interval > 0)
                queryString.Append($"interval={parameters.Interval}&");
            if (!string.IsNullOrEmpty(parameters.Periodicity))
                queryString.Append($"periodicity={Uri.EscapeDataString(parameters.Periodicity)}&");
            if (!string.IsNullOrEmpty(parameters.StartDate))
                queryString.Append($"startDate={Uri.EscapeDataString(parameters.StartDate)}&");
            if (!string.IsNullOrEmpty(parameters.EndDate))
                queryString.Append($"endDate={Uri.EscapeDataString(parameters.EndDate)}&");
            if (queryString[queryString.Length - 1] == '&')
                queryString.Length--;
            return queryString.ToString();
        }
        public async Task<BarsApiResponse> GetBarsTimeBackAsync(BarsTimeBackQueryParameters queryParams)
        {
            await SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync(GetBarsTimeBackRangeUrl(queryParams));

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await ReinitializeAuthorizationAsync(_httpClient);
                response = await _httpClient.GetAsync(GetBarsTimeBackRangeUrl(queryParams));
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
                return await ErrorResponseHandler(response);
            return await SuccessResponseHandler(response);
        }
        private string GetBarsTimeBackRangeUrl(BarsTimeBackQueryParameters parameters)
        {
            var queryString = new StringBuilder($"{_credentials.BaseUrl}/api/bars/v1/bars/date-range?");
            if (!string.IsNullOrEmpty(parameters.InstrumentId))
                queryString.Append($"instrumentId={Uri.EscapeDataString(parameters.InstrumentId)}&");
            if (!string.IsNullOrEmpty(parameters.Provider))
                queryString.Append($"provider={Uri.EscapeDataString(parameters.Provider)}&");
            if (parameters.Interval > 0)
                queryString.Append($"interval={parameters.Interval}&");
            if (!string.IsNullOrEmpty(parameters.Periodicity))
                queryString.Append($"periodicity={Uri.EscapeDataString(parameters.Periodicity)}&");
            if (!string.IsNullOrEmpty(parameters.TimeBack))
                queryString.Append($"startDate={Uri.EscapeDataString(parameters.TimeBack)}&");
            if (queryString[queryString.Length - 1] == '&')
                queryString.Length--;
            return queryString.ToString();
        }
    }
}
