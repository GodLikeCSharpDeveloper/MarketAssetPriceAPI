namespace MarketAssetPriceAPI.Models.Instruments
{
    public class InstrumentQueryParameters
    {
        public string Provider { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
