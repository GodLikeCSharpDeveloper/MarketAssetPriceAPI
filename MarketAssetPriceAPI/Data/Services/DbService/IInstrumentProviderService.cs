using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public interface IInstrumentProviderService
    {
        public Task AddNewInstrumentProvider(InstrumentProviderRelationEntity instrumentProvider);
        public Task AddNewInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders);
    }
}
