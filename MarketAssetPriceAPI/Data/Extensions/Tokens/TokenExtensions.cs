using System.IdentityModel.Tokens.Jwt;

namespace MarketAssetPriceAPI.Data.Extensions.Tokens
{
    public static class TokenExtensions
    {

        public static bool IsTokenExpired(this string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken ??
                throw new ArgumentException("Invalid JWT token");
            var exp = (jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value) ??
                throw new ArgumentException("JWT token does not have an exp claim");
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp)).UtcDateTime;
            return expirationTime <= DateTime.UtcNow;
        }
    }
}
