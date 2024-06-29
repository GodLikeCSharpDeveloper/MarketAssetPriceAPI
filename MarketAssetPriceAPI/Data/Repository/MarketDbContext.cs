using MarketAssetPriceAPI.Data.Models.DTOs;
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
            modelBuilder.Entity<InstrumentProviderRelationEntity>()
                .HasIndex(ip => new { ip.ProviderId, ip.InstrumentId })
                .IsUnique();
            modelBuilder.Entity<InstrumentProviderRelationEntity>()
               .HasOne<ProviderEntity>()
               .WithMany()
               .HasForeignKey(ip => ip.ProviderId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<InstrumentProviderRelationEntity>()
                .HasOne<InstrumentEntity>()
                .WithMany()
                .HasForeignKey(ip => ip.InstrumentId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<InstrumentProviderRelationEntity>()
                .HasKey(ip => new { ip.ProviderId, ip.InstrumentId });
            modelBuilder.Entity<InstrumentEntity>()
                .HasMany<ProviderEntity>()
                .WithMany()
                .UsingEntity<InstrumentProviderRelationEntity>(j => j.ToTable("InstrumentProviders"));
            modelBuilder.Entity<InstrumentProviderRelationEntity>()
                .HasOne<ProviderEntity>()
                .WithMany()
                .HasForeignKey(ip => ip.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ExchangeEntity>()
                .HasOne<ProviderEntity>()
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
