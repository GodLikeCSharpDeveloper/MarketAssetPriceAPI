using Newtonsoft.Json;

namespace MarketAssetPriceAPI.Models
{
    public class FintachartCredentials
    { 
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BaseUrl { get; set; }
    }
}
