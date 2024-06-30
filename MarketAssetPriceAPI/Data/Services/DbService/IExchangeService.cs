using MarketAssetPriceAPI.Data.Models.Entities;
using MarketAssetPriceAPI.Data.Repository;

namespace MarketAssetPriceAPI.Data.Services.DbService
{
    public interface IExchangeService
    {
        public Task<ExchangeEntity> GetOrAddExchangeEntity(ExchangeEntity exchangeEntity);
        public Task<ExchangeEntity> GetExchangeEntityByName(string exchangeName);
        public Task<ExchangeEntity> AddExchangeAsync(ExchangeEntity exchangeEntity);
    }
}
