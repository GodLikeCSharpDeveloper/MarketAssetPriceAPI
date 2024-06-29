using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Exchanges;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;
using MarketAssetPriceAPI.Data.Services.DbService;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Web;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public interface IInstrumentControllerService
    {
        public Task<InstrumentsResponse> GetInstruments(InstrumentQueryParameters parameters);
        public Task<ExchangeResponse> GetExchanges(ExchangesQueryParameters parameters);
        public Task<Providers> GetProviders();
    }
}
