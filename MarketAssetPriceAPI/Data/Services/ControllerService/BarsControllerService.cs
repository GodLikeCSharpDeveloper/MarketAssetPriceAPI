using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using MarketAssetPriceAPI.Data.Models.ConnectionModels;
using MarketAssetPriceAPI.Data.Services;
using MarketAssetPriceAPI.Models.Bars;
using MarketAssetPriceAPI.Models.Bars.QueryParameters;
using System.Web;


public class BarsControllerService(HttpClient httpClient, IOptions<FintachartCredentials> credentials, TokenControllerService tokenService) : AuthorizedControllerService(tokenService)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly FintachartCredentials _credentials = credentials.Value;

    public async Task<BarsApiResponse> GetBarsDataAsync(IBarsQueryParameters queryParams)
    {
        await SetAuthorizationHeaderAsync(_httpClient);
        var response = await _httpClient.GetAsync(BuildBarsQuery(queryParams));

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await ReinitializeAuthorizationAsync(_httpClient);
            response = await _httpClient.GetAsync(BuildBarsQuery(queryParams));
        }
        if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
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
    private string BuildBarsQuery(IBarsQueryParameters parameters)
    {
        var queryString = InitialBaseQueryBuild(parameters);
        var resultUrl = AdditionalAppend(parameters, queryString).ToString();
        return resultUrl;
    }
    private static UriBuilder AdditionalAppend(IBarsQueryParameters parameters, UriBuilder uriBuilder)
    {
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        if (!string.IsNullOrEmpty(parameters.InstrumentId))
            query["instrumentId"] = parameters.InstrumentId;
        if (!string.IsNullOrEmpty(parameters.Provider))
            query["provider"] = parameters.Provider;
        if (parameters.Interval > 0)
            query["interval"] = parameters.Interval.ToString();
        if (!string.IsNullOrEmpty(parameters.Periodicity))
            query["periodicity"] = parameters.Periodicity;

        uriBuilder.Query = query.ToString();

        return uriBuilder;
    }
    private UriBuilder InitialBaseQueryBuild(IBarsQueryParameters parameters)
    {
        var uriBuilder = new UriBuilder(_credentials.BaseUrl);
        string endpoint = "api/bars/v1/bars/";

        switch (parameters)
        {
            case BarsCountBackQueryParameters countParams:
                uriBuilder.Path += endpoint + "count-back";
                if (countParams.BarsCount > 0)
                    uriBuilder.Query += $"barsCount={countParams.BarsCount}";
                break;

            case BarsDateRangeQueryParameters dateRangeParams:
                uriBuilder.Path += endpoint + "date-range";
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                if (!string.IsNullOrEmpty(dateRangeParams.StartDate))
                    query["startDate"] = dateRangeParams.StartDate;
                if (!string.IsNullOrEmpty(dateRangeParams.EndDate))
                    query["endDate"] = dateRangeParams.EndDate;
                uriBuilder.Query = query.ToString();
                break;

            case BarsTimeBackQueryParameters timeBackParams:
                uriBuilder.Path += endpoint + "time-back";
                if (!string.IsNullOrEmpty(timeBackParams.TimeBack))
                    uriBuilder.Query += $"timeBack={Uri.EscapeDataString(timeBackParams.TimeBack)}";
                break;
        }
        return uriBuilder;
    }

    private string ConstructResultURL(StringBuilder queryString)
    {
        if (queryString[queryString.Length - 1] == '&')
            queryString.Length--;
        var resultString = queryString.ToString();
        return resultString;
    }
}
