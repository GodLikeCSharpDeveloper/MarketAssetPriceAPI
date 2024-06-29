namespace MarketAssetPriceAPI.Data.Models.DTOs
{
    public class ProviderEntity
    {
        public int Id { get; set; }
        public string? ProviderName { get; set; }
        public string? Symbol { get; set; }
        public string? Exchange { get; set; }
        public int? DefaultOrderSize { get; set; }
    }
}
