using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Models.Instruments;
using MarketAssetPriceAPI.Data.Models.Providers;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }
        public DbSet<InstrumentEntity> Instruments { get; set; }
        public DbSet<ProviderEntity> Providers { get; set; }
        public DbSet<InstrumentProviderRelationEntity> InstrumentProviderRelations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstrumentProviderRelationEntity>()
            .HasKey(ip => new { ip.ProviderId, ip.InstrumentId });
            modelBuilder.Entity<InstrumentEntity>()
            .HasMany<ProviderEntity>()
            .WithMany()
            .UsingEntity<InstrumentProviderRelationEntity>(j => j.ToTable("InstrumentProviders"));
        }
    }
}
