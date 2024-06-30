using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public interface IProviderService
    {
        public Task<ProviderEntity> AddNewProvider(ProviderEntity provider);
        public Task<List<ProviderEntity>> AddNewProviders(List<ProviderEntity> providerValues);
    }
}
