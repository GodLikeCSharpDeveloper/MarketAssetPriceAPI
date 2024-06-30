using MarketAssetPriceAPI.Data.Controllers;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.CountBack;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    public class AuthorizedControllerServiceTests
    {
        private AuthorizedControllerService _controller;
        private Mock<ITokenControllerService> _mockTokenService;
        private Mock<HttpClient> _mockHttpClient;
        [SetUp]
        public void Setup()
        {
            _mockTokenService = new Mock<ITokenControllerService>();
            
            _mockHttpClient = new Mock<HttpClient>();
        }

        [Test]
        public async Task SetAuthorizationHeaderAsync_SetsAuthorizationHeader()
        {
            // Arrange
            var mockTokenService = new Mock<ITokenControllerService>();
            var accessToken = "test-access-token";
            mockTokenService.Setup(s => s.GetAccessToken()).ReturnsAsync(accessToken);

            _controller = new AuthorizedControllerService(mockTokenService.Object);

            // Act
            await _controller.SetAuthorizationHeaderAsync(_mockHttpClient.Object);

            // Assert
            var authHeader = _mockHttpClient.Object.DefaultRequestHeaders.Authorization;
            Assert.That(authHeader, Is.Not.Null);
            Assert.That(authHeader.Scheme, Is.EqualTo("Bearer"));
            Assert.That(authHeader.Parameter, Is.EqualTo(accessToken));

            mockTokenService.Verify(s => s.GetAccessToken(), Times.Once);
        }
        [Test]
        public async Task ReinitializeAuthorizationAsync_ReinitializesAndSetsAuthorizationHeader()
        {
            // Arrange
            var mockTokenService = new Mock<ITokenControllerService>();
            var accessToken = "test-access-token";
            mockTokenService.Setup(s => s.GetAccessToken()).ReturnsAsync(accessToken);
            mockTokenService.Setup(s => s.ReinitializeAuthorization()).Returns(Task.CompletedTask);

            _controller = new AuthorizedControllerService(mockTokenService.Object);

            // Act
            await _controller.ReinitializeAuthorizationAsync(_mockHttpClient.Object);

            // Assert
            var authHeader = _mockHttpClient.Object.DefaultRequestHeaders.Authorization;
            Assert.That(authHeader, Is.Not.Null);
            Assert.That(authHeader.Scheme, Is.EqualTo("Bearer"));
            Assert.That(authHeader.Parameter, Is.EqualTo(accessToken));

            mockTokenService.Verify(s => s.ReinitializeAuthorization(), Times.Once);
        }
    }
}