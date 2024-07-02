using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;
using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.MapperService;
using System.Diagnostics.Metrics;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class ProviderService(IProviderRepository providerRepository, IExchangeService exchangeService): IProviderService
    {
        private readonly IProviderRepository providerRepository = providerRepository;
        public async Task<ProviderEntity> AddNewProvider(ProviderEntity provider)
        {
            var existingExchange = await exchangeService.GetOrAddExchangeEntity(provider.Exchange);
            provider.ExchangeId = existingExchange != null ? existingExchange.Id : null;
            provider.Exchange = null;        
            await providerRepository.AddNewProvider(provider);
            return provider;
        }
        public async Task<ProviderEntity> AddOrGetNewProvider(ProviderEntity provider)
        {
            var existingProvider = await providerRepository.GetProviderEntityByProperties(provider);
            if (existingProvider != null)
                return existingProvider;
            var existingExchange = await exchangeService.GetOrAddExchangeEntity(provider.Exchange);
            provider.ExchangeId = existingExchange != null ? existingExchange.Id : null;
            provider.Exchange = null;
            await providerRepository.AddNewProvider(provider);
            return provider;
        }
        public async Task<List<ProviderEntity>> AddNewProviders(List<ProviderEntity> providerValues)
        {
            providerValues.ForEach(async p =>
            {
            var existingExchange = await exchangeService.GetOrAddExchangeEntity(p.Exchange);
                p.ExchangeId = existingExchange != null ? existingExchange.Id : null;
                p.Exchange = null;
            });
            await providerRepository.AddNewProviders(providerValues);
            return providerValues;
        }
    }
}
