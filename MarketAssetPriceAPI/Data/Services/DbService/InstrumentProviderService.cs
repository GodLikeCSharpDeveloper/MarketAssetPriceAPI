using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class InstrumentProviderService(IInstrumentProviderRepository instrumentProviderRepository) : IInstrumentProviderService
    {
        private readonly IInstrumentProviderRepository instrumentProviderRepository = instrumentProviderRepository;
        public async Task AddNewInstrumentProvider(InstrumentProviderRelationEntity instrumentProvider)
        {
            if (instrumentProvider == null || instrumentProvider.ProviderId == 0 || instrumentProvider.InstrumentId == 0)
                return;
            await instrumentProviderRepository.AddNewInstrumentProvider(instrumentProvider);
        }
        public async Task AddNewInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            if (instrumentProviders == null || instrumentProviders.Any(d => d.InstrumentId == 0 || d.InstrumentId == 0))
                return;
            await instrumentProviderRepository.AddNewInstrumentProviders(instrumentProviders);
        }
    }
}
