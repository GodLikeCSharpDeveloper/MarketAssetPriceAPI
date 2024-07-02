using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class InstrumentProviderService(IInstrumentProviderRepository instrumentProviderRepository) : IInstrumentProviderService
    {
        private readonly IInstrumentProviderRepository instrumentProviderRepository = instrumentProviderRepository;
        public async Task AddNewInstrumentProvider(InstrumentProviderRelationEntity instrumentProvider)
        {
            try
            {
                if (instrumentProvider == null || instrumentProvider.ProviderId == 0 || instrumentProvider.InstrumentId == 0)
                    return;
                await instrumentProviderRepository.AddNewInstrumentProvider(instrumentProvider);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<List<InstrumentProviderRelationEntity>> AddNewInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            try
            {
                if (instrumentProviders == null || instrumentProviders.Any(d => d.InstrumentId == 0 || d.InstrumentId == 0))
                    return [];
                await instrumentProviderRepository.AddNewInstrumentProviders(instrumentProviders);
                return instrumentProviders;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }         
        }
        public async Task<List<InstrumentProviderRelationEntity>> AddNewInstrumentProvidersIfNotExistAlready(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            var existingProviders = await instrumentProviderRepository.GetInstrumentProviderByInstrumentAndProviderId(instrumentProviders);

            var newProviders = instrumentProviders
                .Where(ip => !existingProviders
                    .Any(ep => ep.InstrumentId == ip.InstrumentId && ep.ProviderId == ip.ProviderId))
                .ToList();
            if (newProviders.Count != 0)
            
                await instrumentProviderRepository.AddNewInstrumentProviders(newProviders);
            return newProviders;
        }
        public async Task AddOrUpdateInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            try
            {
                var instrumentIds = instrumentProviders.Select(d => d.InstrumentId).ToList();
                var existingRelations = await instrumentProviderRepository.GetInstrumentProviderByInstrumentIds(instrumentIds);
                var relationsToAdd = instrumentProviders.
                    Where(ip => !existingRelations.
                    Any(er => er.InstrumentId == ip.InstrumentId && er.ProviderId == ip.ProviderId)).ToList();
                var relationsToRemove = existingRelations.
                    Where(er => !instrumentProviders.
                    Any(ip => ip.InstrumentId == er.InstrumentId && ip.ProviderId == er.ProviderId)).ToList();
                var relationsToUpdate = instrumentProviders.
                    Where(ip => existingRelations.
                    Any(er => er.InstrumentId == ip.InstrumentId && er.ProviderId == ip.ProviderId)).ToList();
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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
