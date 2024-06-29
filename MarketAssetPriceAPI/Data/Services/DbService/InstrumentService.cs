using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.MapperService;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class InstrumentService(IInstrumentRepository instrumentRepository, 
        IProviderService providerService,
        IInstrumentProviderService instrumentProviderService,
        IExchangeService exchangeService) : IInstrumentService
    {
        private readonly IInstrumentRepository instrumentRepository = instrumentRepository;
        private readonly IProviderService providerService = providerService;
        private readonly IInstrumentProviderService instrumentProviderService = instrumentProviderService;
        private readonly IExchangeService exchangeService = exchangeService;
        public async Task AddNewInstrument(Instrument instrument)
        {
            var instrumentEntity = InstrumentMapper.MapInstrumentEntity(instrument);
            var providers = await providerService.AddNewProviders(instrumentEntity.Providers);
            await instrumentRepository.AddNewInstrument(instrumentEntity);
            var instrumentProviders = new List<InstrumentProviderRelationEntity>();
            foreach (var provider in providers)
                instrumentProviders.Add(new InstrumentProviderRelationEntity() { InstrumentId = instrumentEntity.Id, ProviderId = provider.Id });
            await instrumentProviderService.AddNewInstrumentProviders(instrumentProviders);
        }
        public async Task AddNewInstruments(List<Instrument> instruments)
        {
            var instrumentEntities = new List<InstrumentEntity>();
            instruments.ForEach(inst => instrumentEntities.Add(InstrumentMapper.MapInstrumentEntity(inst)));
            instrumentEntities = await instrumentRepository.AddNewInstruments(instrumentEntities);
            var instrumentProviders = new List<InstrumentProviderRelationEntity>();
            instrumentEntities.ForEach(async (ent) =>
            {
                ent.Providers.ForEach(async (pr) => pr.ExchangeId = (await exchangeService.GetOrAddExchangeEntity(new() { ExchangeName = pr.Exchange })).Id);
                ent.Providers = await providerService.AddNewProviders(ent.Providers);                          
                ent.Providers.ForEach(provider => { instrumentProviders.Add(new() { InstrumentId = ent.Id, ProviderId = provider.Id }); });
            });
            await instrumentProviderService.AddNewInstrumentProviders(instrumentProviders);
        }
    }
}
