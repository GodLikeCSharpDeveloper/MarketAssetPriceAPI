using MarketAssetPriceAPI.Data.Extensions.Tokens;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.ConnectionModels;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    public class TokenControllerServiceTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<IOptions<FintachartCredentials>> _mockCredentials;
        private Mock<TokenResponseStore> _mockTokenResponse;
        private HttpClient _httpClient;
        private TokenControllerService _service;

        [SetUp]
        public void Setup()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockCredentials = new Mock<IOptions<FintachartCredentials>>();
            _mockTokenResponse = new Mock<TokenResponseStore>();

            var credentials = new FintachartCredentials { BaseUrl = "https://api.test.com", UserName = "testuser", Password = "testpass" };
            _mockCredentials.Setup(c => c.Value).Returns(credentials);

            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient(new MockHttpMessageHandler()));

            _service = new TokenControllerService(_mockHttpClientFactory.Object, _mockCredentials.Object, _mockTokenResponse.Object);
        }

        private class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new TokenResponseStore
                    {
                        AccessToken = "new_access_token",
                        RefreshToken = "new_refresh_token"
                    }), Encoding.UTF8, "application/json")
                };
                return Task.FromResult(response);
            }
        }

        [Test]
        public async Task GetAccessToken_WhenCalled_ReturnsAccessToken()
        {
            // Arrange
            _mockTokenResponse.Object.AccessToken = TokenTestGenerator.GenerateTokenForTests(DateTime.UtcNow.AddMinutes(-1));
            _mockTokenResponse.Object.RefreshToken = TokenTestGenerator.GenerateTokenForTests(DateTime.UtcNow.AddMinutes(-1));

            // Act
            var token = await _service.GetAccessToken();

            // Assert
            Assert.That(token, Is.EqualTo("new_access_token"));
            Assert.That(_mockTokenResponse.Object.AccessToken, Is.EqualTo("new_access_token"));
            Assert.That(_mockTokenResponse.Object.RefreshToken, Is.EqualTo("new_refresh_token"));
        }

        [Test]
        public async Task ReinitializeAuthorization_WhenCalled_SetsNewTokens()
        {
            // Arrange
            _mockTokenResponse.Object.AccessToken = TokenTestGenerator.GenerateTokenForTests(DateTime.UtcNow.AddMinutes(-1));
            _mockTokenResponse.Object.RefreshToken = TokenTestGenerator.GenerateTokenForTests(DateTime.UtcNow.AddMinutes(-1));

            // Act
            await _service.ReinitializeAuthorization();

            // Assert
            Assert.That(_mockTokenResponse.Object.AccessToken, Is.EqualTo("new_access_token"));
            Assert.That(_mockTokenResponse.Object.RefreshToken, Is.EqualTo("new_refresh_token"));
        }
    }
}
