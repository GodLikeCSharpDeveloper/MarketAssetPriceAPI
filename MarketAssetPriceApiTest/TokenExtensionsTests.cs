using MarketAssetPriceAPI.Data.Extensions.Tokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    internal class TokenExtensionsTests
    {
            public static byte[] GenerateBase64EncodedKey(int size = 32)
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] key = new byte[size];
                    rng.GetBytes(key);
                    return key;
                }
            }
        private static string GenerateTokenForTests(DateTime expiration)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(GenerateBase64EncodedKey());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [Test]
        public void IsTokenExpired_ValidTokenNotExpired_ReturnsFalse()
        {
            // Arrange
            var token = GenerateTokenForTests(DateTime.UtcNow.AddMinutes(10));

            // Act
            var result = token.IsTokenExpired();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsTokenExpired_ValidTokenExpired_ReturnsTrue()
        {
            // Arrange
            var token = GenerateTokenForTests(DateTime.UtcNow.AddMinutes(-10));

            // Act
            var result = token.IsTokenExpired();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsTokenExpired_InvalidToken_ThrowsArgumentException()
        {
            // Arrange
            var token = "invalid.token.value";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => token.IsTokenExpired(), "Invalid JWT token");
        }

        [Test]
        public void IsTokenExpired_TokenWithoutExpClaim_ThrowsArgumentException()
        {
            // Arrange
            var handler = new JwtSecurityTokenHandler();
            var securityToken = new JwtSecurityToken();
            var token = handler.WriteToken(securityToken);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => token.IsTokenExpired(), "JWT token does not have an exp claim");
        }

    }
}
