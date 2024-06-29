using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.MapperService;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class InstrumentService(InstrumentRepository instrumentRepository, ProviderService providerService, InstrumentProviderService instrumentProviderService)
    {
        private readonly InstrumentRepository instrumentRepository = instrumentRepository;
        private readonly ProviderService providerService = providerService;
        private readonly InstrumentProviderService instrumentProviderService = instrumentProviderService;
        public async Task AddNewInstrumentAsync(Instrument instrument)
        {
            var instrumentEntity = InstrumentMapper.MapInstrumentEntity(instrument);
            var providers = await providerService.AddNewProvidersAsync(instrumentEntity.Providers);
            await instrumentRepository.AddNewInstrument(instrumentEntity);
            var instrumentProviders = new List<InstrumentProviderRelationEntity>();
            foreach (var provider in providers)
                instrumentProviders.Add(new InstrumentProviderRelationEntity() { InstrumentId = instrumentEntity.Id, ProviderId = provider.Id });
            await instrumentProviderService.AddNewInstrumentProviders(instrumentProviders);
        }
        public async Task AddNewInstrumentsAsync(List<Instrument> instruments)
        {
            var instrumentEntities = new List<InstrumentEntity>();
            instruments.ForEach(inst => instrumentEntities.Add(InstrumentMapper.MapInstrumentEntity(inst)));
            var apiProviderIds = instrumentEntities.Select(i => i.ApiProviderId).ToList();
            if (apiProviderIds.Any())
            {
                var existingInstruments = await instrumentRepository.GetInstrumentsByApiProviderIdsAsync(apiProviderIds);
                var instrumentToAdd = instrumentEntities.Select(d => existingInstruments.Any(e=>e.ApiProviderId == d.ApiProviderId));
                instrumentToAdd = await instrumentRepository.AddNewInstruments(instrumentToAdd);
                var instrumentProviders = new List<InstrumentProviderRelationEntity>();
                instrumentEntities.ForEach(async (ent) =>
                {
                    ent.Providers = await providerService.AddNewProvidersAsync(ent.Providers);
                    ent.Providers.ForEach(provider => { instrumentProviders.Add(new() { InstrumentId = ent.Id, ProviderId = provider.Id }); });
                });
                await instrumentProviderService.AddNewInstrumentProviders(instrumentProviders);
            }
        }
    }
}
