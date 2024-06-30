using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.MapperService;
using System.Diagnostics.Metrics;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class ProviderService(IProviderRepository providerRepository): IProviderService
    {
        private readonly IProviderRepository providerRepository = providerRepository;
        public async Task<ProviderEntity> AddNewProvider(ProviderEntity provider)
        {
            await providerRepository.AddNewProvider(provider);
            return provider;
        }
        public async Task<List<ProviderEntity>> AddNewProviders(List<ProviderEntity> providerValues)
        {      
            await providerRepository.AddNewProviders(providerValues);
            return providerValues;
        }
    }
}
