using MarketAssetPriceAPI.Data.Models.DTOs;
using MarketAssetPriceAPI.Data.Models.Instruments;
using MarketAssetPriceAPI.Data.Models.Providers;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceAPI.Data.Repository
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }
        public DbSet<InstrumentDTO> Instruments { get; set; }
        public DbSet<ProviderDTO> Providers { get; set; }
        public DbSet<InstrumentProviderRelation> InstrumentProviderRelations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstrumentProviderRelation>()
            .HasKey(ip => new { ip.ProviderId, ip.InstrumentId });

            modelBuilder.Entity<InstrumentProviderRelation>()
                .HasOne<ProviderDTO>()
                .WithMany(p => p.InstrumentProviders)
                .HasForeignKey(ip => ip.ProviderId);

            modelBuilder.Entity<InstrumentProviderRelation>()
                .HasOne<InstrumentDTO>()
                .(ip => ip.InstrumentId);

            modelBuilder.Entity<InstrumentProviderRelation>()
                .ToTable("InstrumentProvider");
        }
    }
}
