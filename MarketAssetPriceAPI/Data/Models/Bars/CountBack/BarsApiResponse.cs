namespace MarketAssetPriceAPI.Models.Bars
{
    public class BarsApiResponse
    {
        public bool IsSuccess { get; set; }
        public BarsCountResponse SuccessResponse { get; set; }
        public ErrorResponse ErrorResponse { get; set; }
    }
}
