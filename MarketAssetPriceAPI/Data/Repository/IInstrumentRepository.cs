using MarketAssetPriceAPI.Data.Models.DTOs;

namespace MarketAssetPriceAPI.Data.Repository
{
    public interface IInstrumentRepository
    {
        public Task<InstrumentEntity> AddNewInstrument(InstrumentEntity instrument);
        public Task<List<InstrumentEntity>> AddNewInstruments(List<InstrumentEntity> instruments);
        public Task<List<InstrumentEntity>> GetInstrumentsByApiProviderIds(List<string> apiProviderIds);
        public Task SaveChangesAsync();
    }
}
