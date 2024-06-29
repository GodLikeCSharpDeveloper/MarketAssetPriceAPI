using MarketAssetPriceAPI.Data.Models.DTOs;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class ProviderRepository(MarketDbContext marketDbContext)
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task<ProviderEntity> AddNewProviderAsync(ProviderEntity provider)
        {
            await marketDbContext.AddAsync(provider);
            return provider;
        }
    }
}
