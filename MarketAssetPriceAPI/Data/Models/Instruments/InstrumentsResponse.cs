using System.Net.NetworkInformation;
using MarketAssetPriceAPI.Data.Models;
using Newtonsoft.Json;

namespace MarketAssetPriceAPI.Data.Models.Instruments
{
    public class InstrumentsResponse
    {
        public Paging Paging { get; set; }
        [JsonProperty("data")]
        public List<Instrument> Instruments { get; set; }
    }
}
