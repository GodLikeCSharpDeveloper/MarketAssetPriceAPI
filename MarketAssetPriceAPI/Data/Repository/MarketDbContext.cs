using MarketAssetPriceAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }
        public DbSet<InstrumentEntity> Instruments { get; set; }
        public DbSet<ProviderEntity> Providers { get; set; }
        public DbSet<InstrumentProviderRelationEntity> InstrumentProviderRelations { get; set; }
        public DbSet<ExchangeEntity> Exchanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // InstrumentEntity configuration
            modelBuilder.Entity<InstrumentEntity>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<InstrumentEntity>()
                .HasMany(i => i.Providers)
                .WithMany(p => p.Instruments)
                .UsingEntity<InstrumentProviderRelationEntity>(
                    j => j
                        .HasOne(ipr => ipr.Provider)
                        .WithMany(p => p.InstrumentProviderRelations)
                        .HasForeignKey(ipr => ipr.ProviderId),
                    j => j
                        .HasOne(ipr => ipr.Instrument)
                        .WithMany(i => i.InstrumentProviderRelations)
                        .HasForeignKey(ipr => ipr.InstrumentId),
                    j =>
                    {
                        j.HasKey(ipr => ipr.Id);
                    });

            // ProviderEntity configuration
            modelBuilder.Entity<ProviderEntity>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ProviderEntity>()
                .HasOne(p => p.Exchange)
                .WithMany()
                .HasForeignKey(p => p.ExchangeId);

            // ExchangeEntity configuration
            modelBuilder.Entity<ExchangeEntity>()
                .HasKey(e => e.Id);

            // InstrumentProviderRelationEntity configuration
            modelBuilder.Entity<InstrumentProviderRelationEntity>()
                .HasKey(ipr => ipr.Id);

            modelBuilder.Entity<InstrumentProviderRelationEntity>()
                .HasOne(ipr => ipr.Provider)
                .WithMany(p => p.InstrumentProviderRelations)
                .HasForeignKey(ipr => ipr.ProviderId);

            modelBuilder.Entity<InstrumentProviderRelationEntity>()
                .HasOne(ipr => ipr.Instrument)
                .WithMany(i => i.InstrumentProviderRelations)
                .HasForeignKey(ipr => ipr.InstrumentId);
        }
    }
}
