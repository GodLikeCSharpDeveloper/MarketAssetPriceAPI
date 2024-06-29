using MarketAssetPriceAPI.Data.Models.DTOs;

namespace MarketAssetPriceAPI.Data.Repository
{
    public interface IProviderRepository
    {
        public Task<ProviderEntity> AddNewProvider(ProviderEntity provider);
        public Task<List<ProviderEntity>> AddNewProviders(List<ProviderEntity> providers);
    }
}
