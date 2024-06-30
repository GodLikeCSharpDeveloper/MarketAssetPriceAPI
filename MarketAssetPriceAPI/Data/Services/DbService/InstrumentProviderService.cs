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
        public async Task AddOrUpdateInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            var instrumentIds = instrumentProviders.Select(d => d.InstrumentId).ToList();
            var existingRelations = await instrumentProviderRepository.GetInstrumentProviderByInstrumentIds(instrumentIds);
            var relationsToAdd = instrumentProviders.Where(ip => !existingRelations.Any(er => er.InstrumentId == ip.InstrumentId && er.ProviderId == ip.ProviderId)).ToList();
            var relationsToRemove = existingRelations.Where(er => !instrumentProviders.Any(ip => ip.InstrumentId == er.InstrumentId && ip.ProviderId == er.ProviderId)).ToList();
            var relationsToUpdate = instrumentProviders.Where(ip => existingRelations.Any(er => er.InstrumentId == ip.InstrumentId && er.ProviderId == ip.ProviderId)).ToList();
            if (relationsToRemove.Count != 0)
            {
                await RemoveInstrumentProviders(relationsToRemove);
            }
            if (relationsToAdd.Count != 0)
            {
                await AddNewInstrumentProviders(relationsToAdd);
            }
            if (relationsToUpdate.Count != 0)
            {
                await UpdateInstrumentProvidersRelations(relationsToUpdate);
            }
        }

        public async Task UpdateInstrumentProvidersRelations(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            await instrumentProviderRepository.UpdateInstrumentProviders(instrumentProviders);

        }

        public async Task RemoveInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProvidersToRemove)
        {
            await instrumentProviderRepository.RemoveInstrumentProviders(instrumentProvidersToRemove);
        }
    }
}
