using MarketAssetPriceAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class ProviderRepository(MarketDbContext marketDbContext) : IProviderRepository
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task<ProviderEntity> AddNewProvider(ProviderEntity provider)
        {
            await marketDbContext.AddAsync(provider);
            await marketDbContext.SaveChangesAsync();
            return provider;
        }
        public async Task<ProviderEntity?> GetProviderEntityByProperties(ProviderEntity provider)
        {
            var existingProvider = await marketDbContext.Providers.Where(p => p.ProviderName == provider.ProviderName && p.Symbol == provider.Symbol && p.DefaultOrderSize == provider.DefaultOrderSize).FirstOrDefaultAsync();
            return existingProvider;
        }
        public async Task<List<ProviderEntity>> AddNewProviders(List<ProviderEntity> providers)
        {
            await marketDbContext.AddRangeAsync(providers);
            await marketDbContext.SaveChangesAsync();
            return providers;
        }
    }
}
