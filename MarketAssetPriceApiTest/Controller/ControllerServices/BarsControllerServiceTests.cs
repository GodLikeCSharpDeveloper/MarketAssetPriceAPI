using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.CountBack;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.ConnectionModels;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    public class BarsControllerServiceTests: IDisposable
    {
        private Mock<ITokenControllerService> _mockTokenService;
        private Mock<IOptions<FintachartCredentials>> _mockCredentials;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private BarsControllerService _service;

        [SetUp]
        public void Setup()
        {
            _mockTokenService = new Mock<ITokenControllerService>();

            var credentials = new FintachartCredentials { BaseUrl = "https://api.test.com" };
            _mockCredentials = new Mock<IOptions<FintachartCredentials>>();
            _mockCredentials.Setup(c => c.Value).Returns(credentials);

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _service = new BarsControllerService(_httpClient, _mockCredentials.Object, _mockTokenService.Object);
        }

        [Test]
        public async Task GetBarsData_ReturnsSuccessResponse()
        {
            // Arrange
            var queryParams = new BarsCountBackQueryParameters { BarsCount = 10 };
            var successResponse = new BarsCountResponse { };
            var responseContent = new StringContent(JsonConvert.SerializeObject(successResponse));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetBarsData(queryParams);

            // Assert          
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.SuccessResponse, Is.Not.Null);
            });
        }

        [Test]
        public async Task GetBarsData_ReturnsUnauthorizedAndRetries()
        {
            // Arrange
            var queryParams = new BarsCountBackQueryParameters { BarsCount = 10 };
            var successResponse = new BarsCountResponse {  };
            var unauthorizedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            var successResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(successResponse))
            };

            _mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(unauthorizedResponse)
                .ReturnsAsync(successResponseMessage);

            _mockTokenService.Setup(t => t.ReinitializeAuthorization()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.GetBarsData(queryParams);

            // Assert          
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.SuccessResponse, Is.Not.Null);
            });
            _mockTokenService.Verify(t => t.ReinitializeAuthorization(), Times.Once);
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetBarsData_ReturnsBadRequestResponse()
        {
            // Arrange
            var queryParams = new BarsCountBackQueryParameters { BarsCount = 10 };
            var errorResponse = new ErrorResponse { };
            var responseContent = new StringContent(JsonConvert.SerializeObject(errorResponse));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = responseContent };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetBarsData(queryParams);

            // Assert
            
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.ErrorResponse, Is.Not.Null);
            });
        }

        [Test]
        public async Task GetBarsData_ReturnsNotFoundResponse()
        {
            // Arrange
            var queryParams = new BarsCountBackQueryParameters { BarsCount = 10 };
            var errorResponse = new ErrorResponse {};
            var responseContent = new StringContent(JsonConvert.SerializeObject(errorResponse));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound) { Content = responseContent };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetBarsData(queryParams);

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.ErrorResponse, Is.Not.Null);
            });
        }
        [Test]
        public async Task GetBarsData_CorrectlyBuildsQueryAndReturnsSuccessResponse()
        {
            // Arrange
            var queryParams = new BarsCountBackQueryParameters { BarsCount = 10 };
            var successResponse = new BarsCountResponse {  };
            var responseContent = new StringContent(JsonConvert.SerializeObject(successResponse), Encoding.UTF8, "application/json");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == "https://api.test.com/api/bars/v1/bars/count-back?barsCount=10"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetBarsData(queryParams);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.SuccessResponse, Is.Not.Null);
            });
        }
        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

}
