using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public interface IInstrumentProviderService
    {
        public Task AddNewInstrumentProvider(InstrumentProviderRelationEntity instrumentProvider);
        public Task<List<InstrumentProviderRelationEntity>> AddNewInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders);
        public Task<List<InstrumentProviderRelationEntity>> AddNewInstrumentProvidersIfNotExistAlready(List<InstrumentProviderRelationEntity> instrumentProviders);
        public Task UpdateInstrumentProvidersRelations(List<InstrumentProviderRelationEntity> instrumentProviders);
    }
}
