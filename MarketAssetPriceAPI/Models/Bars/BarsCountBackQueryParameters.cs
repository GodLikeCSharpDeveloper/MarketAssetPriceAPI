using System.ComponentModel.DataAnnotations;

namespace MarketAssetPriceAPI.Models.Bars
{
    public class BarsCountBackQueryParameters: BaseBarsQueryParameters
    {
        public int BarsCount { get; set; }
    }
}
