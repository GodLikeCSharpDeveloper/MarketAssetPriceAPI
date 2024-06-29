using Newtonsoft.Json;

namespace MarketAssetPriceAPI.Models.Bars
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
