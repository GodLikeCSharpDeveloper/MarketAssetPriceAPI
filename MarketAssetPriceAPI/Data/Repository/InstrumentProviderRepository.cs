using MarketAssetPriceAPI.Data.Models.DTOs;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class InstrumentProviderRepository(MarketDbContext marketDbContext): IInstrumentProviderRepository
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task AddNewInstrumentProvider(InstrumentProviderRelationEntity instrumentProvider)
        {
            await marketDbContext.AddAsync(instrumentProvider);
            await marketDbContext.SaveChangesAsync();
        }
        public async Task AddNewInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            await marketDbContext.AddRangeAsync(instrumentProviders);
            await marketDbContext.SaveChangesAsync();
        }
    }
}
