namespace MarketAssetPriceAPI.Data.Models.DTOs
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
        
    }
}
