using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Repository;
using MarketAssetPriceAPI.Data.Services.MapperService;
using System.Diagnostics.Metrics;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class ProviderService(ProviderRepository providerRepository)
    {
        private readonly ProviderRepository providerRepository = providerRepository;
        public async Task<ProviderEntity> AddNewProviderAsync(ProviderEntity provider)
        {
            await providerRepository.AddNewProviderAsync(provider);
            return provider;
        }
        public async Task<List<ProviderEntity>> AddNewProvidersAsync(List<ProviderEntity> providerValues)
        {      
            await providerRepository.AddNewProvidersAsync(providerValues);
            return providerValues;
        }
    }
}
