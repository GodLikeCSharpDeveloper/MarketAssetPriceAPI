using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.CountBack;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using System.Net.Http;
using System.Net;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public interface IBarsControllerService
    {
        public Task<BarsApiResponse> GetBarsData(IBarsQueryParameters queryParams);
    }
}
