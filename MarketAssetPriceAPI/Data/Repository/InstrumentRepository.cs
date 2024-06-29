using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Models.Instruments;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class InstrumentRepository(MarketDbContext marketDbContext)
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task<InstrumentDTO> AddNewInstrument(InstrumentDTO instrument)
        {
            try
            {
                await marketDbContext.AddAsync(instrument);
                await marketDbContext.SaveChangesAsync();
                return instrument;
            }
            catch (Exception ex) 
            {
                throw;
            }
          
        }
    }
}
