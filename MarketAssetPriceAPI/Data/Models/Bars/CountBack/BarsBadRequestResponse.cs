using Newtonsoft.Json;

namespace MarketAssetPriceAPI.Data.Models.Bars.CountBack
{
    public class BarsBadRequestResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public Dictionary<string, string> Errors { get; set; }
    }
}
