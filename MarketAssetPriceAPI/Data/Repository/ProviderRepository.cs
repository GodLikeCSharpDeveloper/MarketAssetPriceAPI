using MarketAssetPriceAPI.Data.Models.DTOs;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class ProviderRepository(MarketDbContext marketDbContext)
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task<ProviderEntity> AddNewProviderAsync(ProviderEntity provider)
        {
            await marketDbContext.AddAsync(provider);
            await marketDbContext.SaveChangesAsync();
            return provider;
        }
        public async Task<List<ProviderEntity>> AddNewProvidersAsync(List<ProviderEntity> providers)
        {
            await marketDbContext.AddRangeAsync(providers);
            await marketDbContext.SaveChangesAsync();
            return providers;
        }
    }
}
