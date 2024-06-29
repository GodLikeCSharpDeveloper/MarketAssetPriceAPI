namespace MarketAssetPriceAPI.Models.Bars.QueryParameters
{
    public class BaseBarsQueryParameters: IBarsQueryParameters
    {
        public string InstrumentId { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public int Interval { get; set; }
        public string Periodicity { get; set; } = string.Empty;
    }
}
