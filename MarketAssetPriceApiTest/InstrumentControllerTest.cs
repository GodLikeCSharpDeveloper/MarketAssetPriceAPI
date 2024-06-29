using MarketAssetPriceAPI.Data.Controllers;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.CountBack;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Exchanges;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Providers;
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
    public class InstrumentControllerTest
    {
        private InstrumentController _controller;
        private Mock<IInstrumentControllerService> _mockInstrumentService;
        [SetUp]
        public void Setup()
        {
            _mockInstrumentService = new Mock<IInstrumentControllerService>();
            _controller = new InstrumentController(_mockInstrumentService.Object);
        }

        [Test]
        public async Task GetBarsCountBack_ReturnsBadRequestResult_WithExpectedData()
        {
            // Arrange
            var parameters = new InstrumentQueryParameters { };
            var expectedData = new InstrumentsResponse {  };
            _mockInstrumentService.Setup(s => s.GetInstruments(It.IsAny<InstrumentQueryParameters>())).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetAllInstruments(parameters) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expectedData));
        }

        [Test]
        public async Task GetBarsDateRange_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var parameters = new ExchangesQueryParameters { };
            var expectedData = new ExchangeResponse { };
            _mockInstrumentService.Setup(s => s.GetExchanges(parameters)).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetAllExchanges(parameters) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expectedData));
        }

        [Test]
        public async Task GetBarsTimeBack_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var expectedData = new Providers { };
            _mockInstrumentService.Setup(s => s.GetProviders()).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetAllProviders() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expectedData));
        }
    }
}
