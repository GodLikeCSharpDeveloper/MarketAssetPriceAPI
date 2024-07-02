using MarketAssetPriceAPI.Data.Models.Entities;

namespace MarketAssetPriceAPI.Data.Repository
{
    public interface IInstrumentProviderRepository
    {
        public Task AddNewInstrumentProvider(InstrumentProviderRelationEntity instrumentProvider);
        public Task AddNewInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders);
        public Task UpdateInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders);
        public Task RemoveInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders);
        public Task<List<InstrumentProviderRelationEntity>> GetInstrumentProviderByInstrumentAndProviderId(List<InstrumentProviderRelationEntity> instrumentProviders);
        public Task<List<InstrumentProviderRelationEntity>> GetInstrumentProviderByInstrumentIds(List<int> instrumentIds);
    }
}
