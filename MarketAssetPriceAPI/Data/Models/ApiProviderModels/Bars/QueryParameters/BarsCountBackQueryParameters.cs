using System.ComponentModel.DataAnnotations;

namespace MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters
{
    public class BarsCountBackQueryParameters : BaseBarsQueryParameters
    {
        public int BarsCount { get; set; }
    }
}
