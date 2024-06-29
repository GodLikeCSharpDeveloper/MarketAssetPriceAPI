using MarketAssetPriceAPI.Data.Controllers;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.CountBack;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Instruments;
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
            var parameters = new BarsCountBackQueryParameters { };
            var expectedData = new InstrumentsResponse {  };
            _mockInstrumentService.Setup(s => s.GetInstruments(It.IsAny<InstrumentQueryParameters>())).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetAllInstruments(parameters) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo(expectedData));
        }

        [Test]
        public async Task GetBarsDateRange_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var parameters = new BarsDateRangeQueryParameters { };
            var expectedData = new BarsApiResponse { };
            _mockBarsService.Setup(s => s.GetBarsData(parameters)).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsDateRange(parameters) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo(expectedData));
        }

        [Test]
        public async Task GetBarsTimeBack_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var parameters = new BarsTimeBackQueryParameters { };
            var expectedData = new BarsApiResponse { };
            _mockBarsService.Setup(s => s.GetBarsData(parameters)).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsDateRange(parameters) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo(expectedData));
        }
    }
}
