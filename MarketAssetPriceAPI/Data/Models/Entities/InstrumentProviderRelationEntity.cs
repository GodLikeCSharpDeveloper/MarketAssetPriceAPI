using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MarketAssetPriceAPI.Data.Models.Entities
{
    public class InstrumentProviderRelationEntity
    {
        [Key]
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int InstrumentId { get; set; }
        public ProviderEntity Provider { get; set; }
        public InstrumentEntity Instrument { get; set; }
    }
}
