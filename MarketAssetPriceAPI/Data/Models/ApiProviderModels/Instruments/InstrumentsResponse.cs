using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments
{
    public class InstrumentsResponse
    {
        public Paging Paging { get; set; }
        [JsonProperty("data")]
        public List<Instrument> Instruments { get; set; }
    }
}
