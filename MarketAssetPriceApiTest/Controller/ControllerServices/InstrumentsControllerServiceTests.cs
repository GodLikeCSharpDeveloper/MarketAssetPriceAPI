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
using MarketAssetPriceAPI.Data.Services.DbService;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using Microsoft.AspNetCore.Mvc;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Exchanges;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    public class InstrumentControllerServiceTests : IDisposable
    {
        private Mock<ITokenControllerService> _mockTokenService;
        private Mock<IOptions<FintachartCredentials>> _mockCredentials;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private InstrumentControllerService _service;
        private Mock<IInstrumentService> _mockInstrumentService;
        [SetUp]
        public void Setup()
        {
            _mockTokenService = new Mock<ITokenControllerService>();

            var credentials = new FintachartCredentials { BaseUrl = "https://api.test.com" };
            _mockCredentials = new Mock<IOptions<FintachartCredentials>>();
            _mockCredentials.Setup(c => c.Value).Returns(credentials);
            _mockInstrumentService = new Mock<IInstrumentService>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            _service = new InstrumentControllerService(_mockInstrumentService.Object, _httpClient, _mockTokenService.Object, _mockCredentials.Object);
        }

        [Test]
        public async Task GetInstruments_ReturnsSuccessResponse()
        {
            // Arrange
            var queryParams = new InstrumentQueryParameters { Kind = "KindTest" };
            var successResponse = new InstrumentsResponse { };
            var responseContent = new StringContent(JsonConvert.SerializeObject(successResponse));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetInstruments(queryParams);

            // Assert          
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetInstruments_ReturnsUnauthorizedAndRetries()
        {
            // Arrange
            var queryParams = new InstrumentQueryParameters { Kind = "KindTest" };
            var successResponse = new InstrumentsResponse { };
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
            var result = await _service.GetInstruments(queryParams);

            // Assert          
            Assert.That(result, Is.Not.Null);
            _mockTokenService.Verify(t => t.ReinitializeAuthorization(), Times.Once);
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetInstruments_ReturnsBadRequestResponse()
        {
            // Arrange
            var queryParams = new InstrumentQueryParameters { Kind = "KindTest" };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = null };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetInstruments(queryParams);

            // Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public async Task GetInstruments_ReturnsNotFoundResponse()
        {
            // Arrange
            var queryParams = new InstrumentQueryParameters { Kind = "KindTest" };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound) { Content = null };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetInstruments(queryParams);

            // Assert           
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task GetExchanges_ReturnsSuccessResponse()
        {
            // Arrange
            var queryParams = new ExchangesQueryParameters { Provider = "ProviderTest" };
            Dictionary<string, List<string>> data = new();
            data.Add("ProviderTest", []);
            var successResponse = new ExchangeResponse { Data = data };
            var responseContent = new StringContent(JsonConvert.SerializeObject(successResponse));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetExchanges(queryParams);

            // Assert          
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(data));
        }

        [Test]
        public async Task GetExchanges_ReturnsUnauthorizedAndRetries()
        {
            // Arrange
            var queryParams = new ExchangesQueryParameters { Provider = "ProviderTest" };
            Dictionary<string, List<string>> data = new();
            data.Add("ProviderTest", []);
            var successResponse = new ExchangeResponse { Data = data };
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
            var result = await _service.GetExchanges(queryParams);

            // Assert          
            Assert.That(result, Is.Not.Null);
            _mockTokenService.Verify(t => t.ReinitializeAuthorization(), Times.Once);
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetExchanges_ReturnsBadRequestResponse()
        {
            // Arrange
            var queryParams = new ExchangesQueryParameters { Provider = "ProviderTest" };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Simulated request exception"));
            // Act
            var result = await _service.GetExchanges(queryParams);

            // Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public async Task GetExchanges_ThrowsHttpRequestException_ReturnsNull()
        {
            // Arrange
            var queryParams = new ExchangesQueryParameters { Provider = "ProviderTest" };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Simulated request exception"));

            // Act
            var result = await _service.GetExchanges(queryParams);

            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task GetProviders_ReturnsSuccessResponse()
        {
            // Arrange
            List<string> returnData = new()
            {"ProviderTest", "ProviderTest2"};
            var successResponse = new Providers { Data = returnData };
            var responseContent = new StringContent(JsonConvert.SerializeObject(successResponse));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetProviders();

            // Assert          
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(returnData));
        }
        [Test]
        public async Task GetExchanges_CorrectlyBuildsQueryAndReturnsSuccessResponse()
        {
            // Arrange
            var queryParams = new ExchangesQueryParameters { Provider = "KindTest" };
            var successResponse = new InstrumentsResponse { };
            var responseContent = new StringContent(JsonConvert.SerializeObject(successResponse));
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            var expectedUri = "https://api.test.com/api/instruments/v1/exchanges?provider=KindTest";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == expectedUri && req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetExchanges(queryParams);

            // Assert          
            Assert.That(result, Is.Not.Null);
        }
        [Test]
        public async Task GetInstruments_CorrectlyBuildsQueryAndReturnsSuccessResponse()
        {
            // Arrange
            var queryParams = new InstrumentQueryParameters { Kind = "KindTest" };
            var successResponse = new InstrumentsResponse { /* Initialize properties if needed */ };
            var responseContent = new StringContent(JsonConvert.SerializeObject(successResponse), Encoding.UTF8, "application/json");
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };

            var expectedUri = "https://api.test.com/api/instruments/v1/instruments?kind=KindTest";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString() == expectedUri && req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.GetInstruments(queryParams);

            // Assert          
            Assert.That(result, Is.Not.Null);

        }

        [Test]
        public async Task GetProviders_ReturnsUnauthorizedAndRetries()
        {
            // Arrange
            List<string> returnData = new()
            {"ProviderTest", "ProviderTest2"};
            var successResponse = new Providers { Data = returnData };
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
            var result = await _service.GetProviders();

            // Assert          
            Assert.That(result, Is.Not.Null);
            _mockTokenService.Verify(t => t.ReinitializeAuthorization(), Times.Once);
            _mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task GetProviders_ReturnsBadRequestResponse()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Simulated request exception"));

            // Act
            var result = await _service.GetProviders();

            // Assert
            Assert.That(result, Is.Null);

        }

        [Test]
        public async Task GetProviders_ThrowsHttpRequestException_ReturnsNull()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Simulated request exception"));

            // Act
            var result = await _service.GetProviders();

            // Assert
            Assert.That(result, Is.Null);
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
