namespace MarketAssetPriceAPI.Data.Models.DTOs
{
    public class ProviderDTO
    {
        public int Id { get; set; }
        public string? ProviderName { get; set; }
        public string? Symbol { get; set; }
        public string? Exchange { get; set; }
    }
}
