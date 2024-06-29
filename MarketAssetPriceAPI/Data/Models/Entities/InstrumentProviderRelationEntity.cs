namespace MarketAssetPriceAPI.Data.Models.DTOs
{
    public class InstrumentProviderRelationEntity
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int InstrumentId { get; set; }
    }
}
