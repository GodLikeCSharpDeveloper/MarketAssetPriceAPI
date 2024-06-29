using MarketAssetPriceAPI.Data.Models.DTOs;
using Microsoft.EntityFrameworkCore;


namespace MarketAssetPriceAPI.Data.Repository
{
    public class InstrumentRepository(MarketDbContext marketDbContext)
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task<InstrumentEntity> AddNewInstrument(InstrumentEntity instrument)
        {
            await marketDbContext.AddAsync(instrument);
            await SaveChangesAsync();
            return instrument;
        }
        public async Task<List<InstrumentEntity>> AddNewInstruments(List<InstrumentEntity> instruments)
        {
            await marketDbContext.AddRangeAsync(instruments);
            await SaveChangesAsync();
            return instruments;
        }
        public async Task<List<InstrumentEntity>> GetInstrumentsByApiProviderIdsAsync(List<string> apiProviderIds)
        {
            return await marketDbContext.Instruments
                .Where(i => apiProviderIds.Contains(i.ApiProviderId))
                .ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await marketDbContext.SaveChangesAsync();
        }
    }
}
