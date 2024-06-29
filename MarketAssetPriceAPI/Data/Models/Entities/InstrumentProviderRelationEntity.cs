using System.ComponentModel.DataAnnotations;

namespace MarketAssetPriceAPI.Data.Models.DTOs
{
    public class InstrumentProviderRelationEntity
    {
        [Key]
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int InstrumentId { get; set; }
    }
}
