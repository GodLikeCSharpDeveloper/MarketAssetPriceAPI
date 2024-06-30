using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class ExchangeService(IExchangeRepository exchangeRepository) : IExchangeService
    {
        private readonly IExchangeRepository exchangeRepository = exchangeRepository;
        public async Task<ExchangeEntity> GetOrAddExchangeEntity(ExchangeEntity exchangeEntity)
        {
            var existingEntity = await GetExchangeEntityByName(exchangeEntity.ExchangeName);
            if (existingEntity == null && !string.IsNullOrEmpty(exchangeEntity.ExchangeName))
                return await AddExchangeAsync(exchangeEntity);
            return exchangeEntity;
        }
        public async Task<ExchangeEntity> GetExchangeEntityByName(string exchangeName)
        {
            var existingEntity = await exchangeRepository.GetEchangeAsync(exchangeName);
            return existingEntity;
        }
        public async Task<ExchangeEntity> AddExchangeAsync(ExchangeEntity exchangeEntity)
        {
            return await exchangeRepository.AddExchangeAsync(exchangeEntity);
        }
    }
}
