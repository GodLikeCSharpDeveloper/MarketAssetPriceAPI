namespace MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters
{
    public class BarsTimeBackQueryParameters : BaseBarsQueryParameters
    {
        private TimeSpan timeBack;
        public string TimeBack
        {
            get => timeBack.ToString(@"d\.hh\:mm\:ss");
            set => timeBack = TimeSpan.Parse(value);
        }
    }
}
