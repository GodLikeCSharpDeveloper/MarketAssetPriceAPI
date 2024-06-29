using System.Net.Http.Headers;

namespace MarketAssetPriceAPI.Data.Services
{
    public abstract class AuthorizedControllerService
    {
        private readonly TokenControllerService _tokenService;

        protected AuthorizedControllerService(TokenControllerService tokenService)
        {
            _tokenService = tokenService;
        }
        protected async Task SetAuthorizationHeaderAsync(HttpClient httpClient)
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        protected async Task ReinitializeAuthorizationAsync(HttpClient httpClient)
        {
            await _tokenService.ReinitializeAuthorizationAsync();
            await SetAuthorizationHeaderAsync(httpClient);
        }
    }

}
