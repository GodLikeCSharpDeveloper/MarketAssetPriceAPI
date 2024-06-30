using System.Net.Http.Headers;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public class AuthorizedControllerService(ITokenControllerService tokenService)
    {
        private readonly ITokenControllerService _tokenService = tokenService;

        public async Task SetAuthorizationHeaderAsync(HttpClient httpClient)
        {
            var accessToken = await _tokenService.GetAccessToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task ReinitializeAuthorizationAsync(HttpClient httpClient)
        {
            await _tokenService.ReinitializeAuthorization();
            await SetAuthorizationHeaderAsync(httpClient);
        }
    }

}
