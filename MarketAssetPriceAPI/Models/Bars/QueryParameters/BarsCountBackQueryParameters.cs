using System.ComponentModel.DataAnnotations;

namespace MarketAssetPriceAPI.Models.Bars.QueryParameters
{
    public class BarsCountBackQueryParameters : BaseBarsQueryParameters
    {
        public int BarsCount { get; set; }
    }
}
