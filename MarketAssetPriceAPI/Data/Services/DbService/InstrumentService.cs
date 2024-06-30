using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;
using MarketAssetPriceAPI.Data.Models.Entities;
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
            var instrumentProviders = MakeNewInstrumentProviderEntities(providers, instrumentEntity.Id);
            await instrumentProviderService.AddNewInstrumentProviders(instrumentProviders);
        }

        public async Task AddNewInstruments(List<Instrument> instruments)
        {
            var instrumentEntities = ConvertInstrumentsToEntities(instruments);
            instrumentEntities = await instrumentRepository.AddNewInstruments(instrumentEntities);
            await HandleInstrumentRelations(instrumentEntities);
        }

        private List<InstrumentEntity> ConvertInstrumentsToEntities(List<Instrument> instruments)
        {
            return instruments.Select(InstrumentMapper.MapInstrumentEntity).ToList();
        }

        private async Task HandleInstrumentRelations(List<InstrumentEntity> instruments)
        {
            var tasks = new List<Task>();

            foreach (var ent in instruments)
            {
                tasks.Add(HandleSingleInstrumentRelation(ent));
            }

            await Task.WhenAll(tasks);
        }

        private async Task HandleSingleInstrumentRelation(InstrumentEntity ent)
        {
            foreach (var provider in ent.Providers)
            {
                provider.ExchangeId = (await exchangeService.GetOrAddExchangeEntity(new ExchangeEntity { ExchangeName = provider.Exchange })).Id;
            }

            ent.Providers = await providerService.AddNewProviders(ent.Providers);
            var instrumentProviders = MakeNewInstrumentProviderEntities(ent.Providers, ent.Id);
            await instrumentProviderService.AddNewInstrumentProviders(instrumentProviders);
        }

        private List<InstrumentProviderRelationEntity> MakeNewInstrumentProviderEntities(List<ProviderEntity> providers, int instrumentId)
        {
            return providers.Select(provider => new InstrumentProviderRelationEntity { InstrumentId = instrumentId, ProviderId = provider.Id }).ToList();
        }
    }
}
