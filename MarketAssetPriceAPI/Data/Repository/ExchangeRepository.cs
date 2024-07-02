using MarketAssetPriceAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class ExchangeRepository(MarketDbContext marketDbContext) : IExchangeRepository
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task<ExchangeEntity> AddExchangeAsync(ExchangeEntity exchange)
        {
            var result = await marketDbContext.Exchanges.AddAsync(exchange);
            await marketDbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<ExchangeEntity?> GetEchangeAsync(string exchangeName)
        {
            return await marketDbContext.Exchanges.AsNoTracking().FirstOrDefaultAsync(d => d.ExchangeName == exchangeName);
        }
    }
}
