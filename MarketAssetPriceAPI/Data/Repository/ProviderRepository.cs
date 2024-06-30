using MarketAssetPriceAPI.Data.Models.Entities;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class ProviderRepository(MarketDbContext marketDbContext): IProviderRepository
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task<ProviderEntity> AddNewProvider(ProviderEntity provider)
        {
            await marketDbContext.AddAsync(provider);
            await marketDbContext.SaveChangesAsync();
            return provider;
        }
        public async Task<List<ProviderEntity>> AddNewProviders(List<ProviderEntity> providers)
        {
            await marketDbContext.AddRangeAsync(providers);
            await marketDbContext.SaveChangesAsync();
            return providers;
        }
    }
}
