﻿using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;
using MarketAssetPriceAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class InstrumentProviderRepository(MarketDbContext marketDbContext) : IInstrumentProviderRepository
    {
        private readonly MarketDbContext marketDbContext = marketDbContext;
        public async Task AddNewInstrumentProvider(InstrumentProviderRelationEntity instrumentProvider)
        {
            await marketDbContext.AddAsync(instrumentProvider);
            await marketDbContext.SaveChangesAsync();
        }
        public async Task AddNewInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            await marketDbContext.AddRangeAsync(instrumentProviders);
            await marketDbContext.SaveChangesAsync();
        }
        public async Task UpdateInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            marketDbContext.UpdateRange(instrumentProviders);
            await marketDbContext.SaveChangesAsync();
        }
        public async Task<List<InstrumentProviderRelationEntity>> GetInstrumentProviderByInstrumentIds(List<int> instrumentIds)
        {
            return await marketDbContext.InstrumentProviderRelations.Where(d => instrumentIds.Contains(d.InstrumentId)).ToListAsync();
        }
        public async Task<List<InstrumentProviderRelationEntity>> GetInstrumentProviderByInstrumentAndProviderId(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            var instrumentIds = instrumentProviders.Select(ip => ip.InstrumentId).ToList();
            var providerIds = instrumentProviders.Select(ip => ip.ProviderId).ToList();
            return await marketDbContext.InstrumentProviderRelations
                .Where(ipr => instrumentIds.Contains(ipr.InstrumentId) && providerIds.Contains(ipr.ProviderId))
                .ToListAsync();
        }
        public async Task RemoveInstrumentProviders(List<InstrumentProviderRelationEntity> instrumentProviders)
        {
            marketDbContext.RemoveRange(instrumentProviders);
            await marketDbContext.SaveChangesAsync();
        }
    }
}
