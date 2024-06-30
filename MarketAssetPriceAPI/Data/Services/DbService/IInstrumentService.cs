using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.MapperService;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public interface IInstrumentService
    {
        public Task AddNewInstrument(Instrument instrument);
        public Task AddNewInstruments(List<Instrument> instruments);
        public Task AddOrUpdateNewInstruments(List<Instrument> instruments);
        public Task UpdateInstruments(List<InstrumentEntity> existingInstruments, List<InstrumentEntity> instrumentToUpdate);
    }
}
