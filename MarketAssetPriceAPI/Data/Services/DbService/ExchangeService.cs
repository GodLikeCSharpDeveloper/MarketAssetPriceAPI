using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public class ExchangeService(IExchangeRepository exchangeRepository) : IExchangeService
    {
        private readonly IExchangeRepository exchangeRepository = exchangeRepository;
        public async Task<ExchangeEntity> GetOrAddExchangeEntity(ExchangeEntity exchangeEntity)
        {
            try
            {
                var existingEntity = await GetExchangeEntityByName(exchangeEntity.ExchangeName);
                if (existingEntity == null && !string.IsNullOrEmpty(exchangeEntity.ExchangeName))
                    return await AddExchangeAsync(exchangeEntity);
                return existingEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<ExchangeEntity> GetExchangeEntityByName(string exchangeName)
        {
            try
            {
                var existingEntity = await exchangeRepository.GetEchangeAsync(exchangeName);
                return existingEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<ExchangeEntity> AddExchangeAsync(ExchangeEntity exchangeEntity)
        {
            try
            {
                return await exchangeRepository.AddExchangeAsync(exchangeEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
