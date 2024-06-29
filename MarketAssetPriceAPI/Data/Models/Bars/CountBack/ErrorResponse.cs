using Newtonsoft.Json;

namespace MarketAssetPriceAPI.Data.Models.Bars.CountBack
{
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public BarsBadRequestResponse Error { get; set; }
    }
}
