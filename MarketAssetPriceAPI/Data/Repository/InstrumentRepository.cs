using MarketAssetPriceAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace MarketAssetPriceAPI.Data.Repository
{
    public class InstrumentRepository(MarketDbContext marketDbContext): IInstrumentRepository
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
        public async Task<List<InstrumentEntity>> GetInstrumentsByApiProviderIds(List<string> apiProviderIds)
        {
            return await marketDbContext.Instruments
                .Where(i => apiProviderIds.Contains(i.ApiProviderId))
                .ToListAsync();
        }
        public async Task<InstrumentEntity> UpdateInstrument(InstrumentEntity instrument)
        {
            marketDbContext.Update(instrument);
            await SaveChangesAsync();
            return instrument;
        }
        public async Task<List<InstrumentEntity>> UpdateInstruments(List<InstrumentEntity> instruments)
        {
            marketDbContext.UpdateRange(instruments);
            await SaveChangesAsync();
            return instruments;
        }
        public async Task SaveChangesAsync()
        {
            await marketDbContext.SaveChangesAsync();
        }
    }
}
