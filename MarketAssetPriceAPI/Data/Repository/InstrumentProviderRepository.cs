using MarketAssetPriceAPI.Data.Models.DTOs;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class InstrumentProviderRepository(MarketDbContext marketDbContext)
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task AddNewInstrumentProvider(InstrumentProviderRelation instrumentProvider)
        {
            await marketDbContext.AddAsync(instrumentProvider);
            await marketDbContext.SaveChangesAsync();
        }
    }
}
