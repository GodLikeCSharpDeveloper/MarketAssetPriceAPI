using MarketAssetPriceAPI.Data.Models.Entities;

namespace MarketAssetPriceAPI.Data.Repository
{
    public interface IExchangeRepository
    {
        public Task<ExchangeEntity> AddExchangeAsync(ExchangeEntity exchange);
        public Task<ExchangeEntity?> GetEchangeAsync(string exchangeName);
    }
}
