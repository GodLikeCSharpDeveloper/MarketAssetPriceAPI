using Newtonsoft.Json;

namespace MarketAssetPriceAPI.Models.Bars
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public BarsBadRequestResponse Error { get; set; }
    }
}
