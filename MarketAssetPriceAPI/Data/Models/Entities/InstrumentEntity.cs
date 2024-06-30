using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketAssetPriceAPI.Data.Models.Entities
{
    public class InstrumentEntity
    {
        public int Id { get; set; }
        public string? ApiProviderId { get; set; }
        public string? Symbol { get; set; }
        public string? Kind { get; set; }
        public string? Description { get; set; }
        public double? TickSize { get; set; }
        public string? Currency { get; set; }
        public string? BaseCurrency { get; set; }
        public DateTime LastUpdateTime { get; set; }
        [NotMapped]
        public List<ProviderEntity> Providers { get; set; }
    }
}
