using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class ExchangeService(IExchangeRepository exchangeRepository): IExchangeService
    {
        private readonly IExchangeRepository exchangeRepository = exchangeRepository;
        public async Task<ExchangeEntity> GetOrAddExchangeEntity(ExchangeEntity exchangeEntity)
        {
            var existingEntity = await exchangeRepository.GetEchangeAsync(exchangeEntity.ExchangeName);
            if (existingEntity != null)
                return existingEntity;
            if (existingEntity == null && !string.IsNullOrEmpty(exchangeEntity.ExchangeName))
                return await exchangeRepository.AddExchangeAsync(exchangeEntity);
            return exchangeEntity;
        }
    }
}
