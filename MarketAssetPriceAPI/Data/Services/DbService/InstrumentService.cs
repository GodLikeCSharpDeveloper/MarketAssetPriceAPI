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
            try
            {
                var instrumentEntity = InstrumentMapper.MapInstrumentEntity(instrument);
                var providers = await providerService.AddNewProviders(instrumentEntity.Providers);
                providers.ForEach(pr => instrumentEntity.InstrumentProviderRelations.Add(new() { ProviderId = pr.Id }));
                await instrumentRepository.AddNewInstrument(instrumentEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task AddNewInstruments(List<Instrument> instruments)
        {
            try
            {
                var instrumentEntities = ConvertInstrumentsToEntities(instruments);
                AddProvidersAndConstructRelations(instrumentEntities);
                instrumentEntities = await instrumentRepository.AddNewInstruments(instrumentEntities);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<List<InstrumentEntity>> AddNewInstruments(List<InstrumentEntity> instruments)
        {
            try
            {
                AddProvidersAndConstructRelations(instruments);
                var instrumentEntities = await instrumentRepository.AddNewInstruments(instruments);
                return instrumentEntities;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private List<InstrumentEntity> ConvertInstrumentsToEntities(List<Instrument> instruments)
        {
            return instruments.Select(InstrumentMapper.MapInstrumentEntity).ToList();
        }
        private void AddProvidersAndConstructRelations(List<InstrumentEntity> instruments)
        {
            instruments.ForEach(async i => { await providerService.AddNewProviders(i.Providers); i.Providers.ForEach(p => i.InstrumentProviderRelations.Add(new() { ProviderId = p.Id })); });
        }

        public async Task AddOrUpdateNewInstruments(List<Instrument> instruments)
        {
            try
            {
                var instrumentEntities = ConvertInstrumentsToEntities(instruments);
                var apiProviderIds = instrumentEntities.Select(d => d.ApiProviderId).ToList();
                var existingInstruments = await instrumentRepository.GetInstrumentsByApiProviderIds(apiProviderIds);

                var instrumentsToAdd = instrumentEntities.Where(i => !existingInstruments.Any(d => d.ApiProviderId == i.ApiProviderId)).ToList();
                var instrumentsToUpdate = instrumentEntities.Where(i => existingInstruments.Any(d => d.ApiProviderId == i.ApiProviderId)).ToList();

                if (instrumentsToAdd.Count != 0)
                {
                    var addedInstruments = await AddNewInstruments(instrumentsToAdd);
                }

                if (instrumentsToUpdate.Count != 0)
                {
                    await UpdateInstruments(existingInstruments, instrumentsToUpdate);
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }
        private async Task<List<ProviderEntity>> AddIfNotAdded(List<ProviderEntity> providers)
        {
            var resultProviders = new List<ProviderEntity>();
            foreach (var provider in providers)
            {
                if (provider.Id == 0) resultProviders.Add(await providerService.AddOrGetNewProvider(provider));
            }
            return resultProviders;
        }
        public async Task UpdateInstruments(List<InstrumentEntity> existingInstruments, List<InstrumentEntity> instrumentToUpdate)
        {
            try
            {
                var instrumentProvider = new List<InstrumentProviderRelationEntity>();
                foreach (var instrument in instrumentToUpdate)
                {
                    var existingInstrument = existingInstruments.FirstOrDefault(i => i.ApiProviderId == instrument.ApiProviderId);
                    if (existingInstrument != null)
                    {
                        existingInstrument.Symbol = instrument.Symbol;
                        existingInstrument.Kind = instrument.Kind;
                        existingInstrument.Description = instrument.Description;
                        existingInstrument.TickSize = instrument.TickSize;
                        existingInstrument.Currency = instrument.Currency;
                        existingInstrument.BaseCurrency = instrument.BaseCurrency;
                        existingInstrument.LastUpdateTime = instrument.LastUpdateTime;
                        existingInstrument = await AddNewProvidersAndHandleRelations(instrument, existingInstrument);
                    }
                }
                await instrumentRepository.UpdateInstruments(existingInstruments);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private async Task<InstrumentEntity> AddNewProvidersAndHandleRelations(InstrumentEntity instrument, InstrumentEntity existingInstrument)
        {
            if (instrument.Providers != null)
            {
                instrument.Providers = await AddIfNotAdded(instrument.Providers);
                instrument.Providers.ForEach(p =>
                {
                    if (!existingInstrument.InstrumentProviderRelations.Any(d => d.InstrumentId == existingInstrument.Id && d.ProviderId == p.Id))
                        existingInstrument.InstrumentProviderRelations.Add(new()
                        {
                            InstrumentId = existingInstrument.Id,
                            ProviderId = p.Id
                        });
                });
            }
            return existingInstrument;
        }
    }
}
