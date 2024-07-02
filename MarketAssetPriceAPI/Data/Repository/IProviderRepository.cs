using MarketAssetPriceAPI.Data.Models.Entities;

namespace MarketAssetPriceAPI.Data.Repository
{
    public interface IProviderRepository
    {
        public Task<ProviderEntity> AddNewProvider(ProviderEntity provider);
        public Task<ProviderEntity?> GetProviderEntityByProperties(ProviderEntity provider);
        public Task<List<ProviderEntity>> AddNewProviders(List<ProviderEntity> providers);
    }
}
