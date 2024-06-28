using System.Net.NetworkInformation;

namespace MarketAssetPriceAPI.Models
{
    public class InstrumentsResponse
    {
        public Paging Paging { get; set; }
        public List<Instrument> Data { get; set; }
    }
}
