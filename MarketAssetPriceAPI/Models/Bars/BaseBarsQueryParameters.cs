namespace MarketAssetPriceAPI.Models.Bars
{
    public class BaseBarsQueryParameters
    {
        public string InstrumentId { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public int Interval { get; set; }
        public string Periodicity { get; set; } = string.Empty;
    }
}
