using MarketAssetPriceAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


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
            var result = await marketDbContext.Instruments.Where(i => apiProviderIds.Contains(i.ApiProviderId)).Include(d=>d.Providers).ToListAsync();
            return result;

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
            await marketDbContext.SaveChangesAsync();
            return instruments;
        }
        public async Task SaveChangesAsync()
        {
            await marketDbContext.SaveChangesAsync();
        }
    }
}
