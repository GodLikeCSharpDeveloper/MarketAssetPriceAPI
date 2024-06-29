using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public interface IProviderService
    {
        public Task<ProviderEntity> AddNewProvider(ProviderEntity provider);
        public Task<List<ProviderEntity>> AddNewProviders(List<ProviderEntity> providerValues);
    }
}
