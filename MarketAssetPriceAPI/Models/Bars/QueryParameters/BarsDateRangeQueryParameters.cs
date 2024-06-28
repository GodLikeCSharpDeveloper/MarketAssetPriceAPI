namespace MarketAssetPriceAPI.Models.Bars.QueryParameters
{
    public class BarsDateRangeQueryParameters : BaseBarsQueryParameters
    {
        private DateTime startDate { get; set; }
        private DateTime endDate { get; set; }
        public string StartDate
        {
            get => startDate.ToString("yyyy-MM-dd");
            set => startDate = DateTime.Parse(value);
        }
        public string EndDate
        {
            get => endDate.ToString("yyyy-MM-dd");
            set => endDate = DateTime.Parse(value);
        }
    }
}
