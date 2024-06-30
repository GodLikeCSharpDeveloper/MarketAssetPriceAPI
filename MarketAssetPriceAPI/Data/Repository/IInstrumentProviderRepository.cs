using MarketAssetPriceAPI.Data.Models.Entities;

namespace MarketAssetPriceAPI.Data.Repository
{
    public interface IInstrumentProviderRepository
    {
        public Task AddNewInstrumentProvider(InstrumentProviderRelationEntity instrumentProvider);
        public Task AddNewInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders);
    }
}
