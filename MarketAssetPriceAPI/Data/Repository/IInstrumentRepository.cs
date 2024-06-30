using MarketAssetPriceAPI.Data.Models.Entities;

namespace MarketAssetPriceAPI.Data.Repository
{
    public interface IInstrumentRepository
    {
        public Task<InstrumentEntity> AddNewInstrument(InstrumentEntity instrument);
        public Task<List<InstrumentEntity>> AddNewInstruments(List<InstrumentEntity> instruments);
        public Task<List<InstrumentEntity>> GetInstrumentsByApiProviderIds(List<string> apiProviderIds);
        public Task<InstrumentEntity> UpdateInstrument(InstrumentEntity instrument);
        public Task<List<InstrumentEntity>> UpdateInstruments(List<InstrumentEntity> instruments);
        public Task SaveChangesAsync();
    }
}
