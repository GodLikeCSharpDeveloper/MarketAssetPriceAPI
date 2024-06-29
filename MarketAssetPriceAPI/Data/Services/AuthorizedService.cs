using System.Net.Http.Headers;

namespace MarketAssetPriceAPI.Data.Services
{
    public abstract class AuthorizedService
    {
        private readonly TokenService _tokenService;

        protected AuthorizedService(TokenService tokenService)
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
