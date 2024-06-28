using Newtonsoft.Json;

namespace MarketAssetPriceAPI.Models
{
    public class Instrument
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Kind { get; set; }
        public string Description { get; set; }
        public double TickSize { get; set; }
        public string Currency { get; set; }
        public string BaseCurrency { get; set; }
        public Dictionary<string, Mapping> Mappings { get; set; }
    }

}
