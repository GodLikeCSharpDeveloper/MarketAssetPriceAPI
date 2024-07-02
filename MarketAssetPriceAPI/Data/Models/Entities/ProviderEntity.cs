using System.ComponentModel.DataAnnotations.Schema;

namespace MarketAssetPriceAPI.Data.Models.Entities
{
    public class ProviderEntity
    {
        public int Id { get; set; }
        public string? ProviderName { get; set; }
        public string? Symbol { get; set; }
        public ExchangeEntity? Exchange { get; set; }
        public int? ExchangeId { get; set; }
        public int? DefaultOrderSize { get; set; }
        public List<InstrumentProviderRelationEntity> InstrumentProviderRelations { get; set; } = [];
        [NotMapped]
        public List<InstrumentEntity> Instruments { get; set; }


    }
}
