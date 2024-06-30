using System.Net.Http;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public interface ITokenControllerService
    {
        public Task<string> GetAccessToken();
        public Task ReinitializeAuthorization();
        public bool IsTokenExpired(string token);
    }
}
