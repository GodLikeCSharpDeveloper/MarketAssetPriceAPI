namespace MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters
{
    public interface IBarsQueryParameters
    {
        string InstrumentId { get; set; }
        string Provider { get; set; }
        int Interval { get; set; }
        string Periodicity { get; set; }
    }
}
