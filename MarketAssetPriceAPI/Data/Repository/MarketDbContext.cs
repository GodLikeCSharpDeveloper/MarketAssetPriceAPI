using MarketAssetPriceAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }
        public DbSet<InstrumentEntity> Instruments { get; set; }
        public DbSet<ExchangeEntity> Exchanges { get; set; }
        public DbSet<ProviderEntity> Providers { get; set; }
        public DbSet<InstrumentProviderRelationEntity> InstrumentProviderRelations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InstrumentProviderRelationEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(ip => new { ip.ProviderId, ip.InstrumentId }).IsUnique();

                entity.HasOne(ip => ip.Provider)
                      .WithMany()
                      .HasForeignKey(ip => ip.ProviderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ip => ip.Instrument)
                      .WithMany()
                      .HasForeignKey(ip => ip.InstrumentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProviderEntity>()
                .HasMany<InstrumentProviderRelationEntity>()
                .WithOne(ip => ip.Provider)
                .HasForeignKey(ip => ip.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InstrumentEntity>()
                .HasMany<InstrumentProviderRelationEntity>()
                .WithOne(ip => ip.Instrument)
                .HasForeignKey(ip => ip.InstrumentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
