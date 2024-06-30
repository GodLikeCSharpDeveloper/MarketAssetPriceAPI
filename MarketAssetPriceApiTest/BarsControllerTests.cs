using MarketAssetPriceAPI.Data.Controllers;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.CountBack;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.Bars.QueryParameters;
using MarketAssetPriceAPI.Data.Models.ApiProviderModels.ConnectionModels;
using MarketAssetPriceAPI.Data.Services;
using MarketAssetPriceAPI.Data.Services.ControllerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest
{
    [TestFixture]
    public class BarsControllerTests
    {
        private BarsController _controller;
        private Mock<IBarsControllerService> _mockBarsService;
        [SetUp]
        public void Setup()
        {
            _mockBarsService = new Mock<IBarsControllerService>();
            _controller = new BarsController(_mockBarsService.Object);
        }

        [Test]
        public async Task GetBarsCountBack_ReturnsBadResult_WithExpectedData()
        {
            // Arrange
            var parameters = new BarsCountBackQueryParameters { };
            var expectedData = new BarsApiResponse{ IsSuccess = false, ErrorResponse = new ErrorResponse() { } };
            _mockBarsService.Setup(s => s.GetBarsData(It.IsAny<BarsCountBackQueryParameters>())).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsCountBack(parameters) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo(expectedData));
            var response = result.Value as BarsApiResponse;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ErrorResponse, Is.Not.Null);
        }
        [Test]
        public async Task GetBarsCountBack_ReturnsBadResult_WhenDataIsNull()
        {
            // Arrange
            var parameters = new BarsCountBackQueryParameters { };
            BarsApiResponse expectedData = null;
            _mockBarsService.Setup(s => s.GetBarsData(It.IsAny<BarsCountBackQueryParameters>())).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsCountBack(parameters) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo(expectedData));
        }
        [Test]
        public async Task GetBarsCountBack_ReturnsOkResult_WithExpectedData()
        {
            // Arrange
            var parameters = new BarsCountBackQueryParameters { };
            var expectedData = new BarsApiResponse { IsSuccess = true, SuccessResponse = new() };
            _mockBarsService.Setup(s => s.GetBarsData(It.IsAny<BarsCountBackQueryParameters>())).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsCountBack(parameters) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expectedData));
            var response = result.Value as BarsApiResponse;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.SuccessResponse, Is.Not.Null);
        }
        [Test]
        public async Task GetBarsDateRange_ReturnsBadResult_WithExpectedData()
        {
            // Arrange
            var parameters = new BarsDateRangeQueryParameters {  };
            var expectedData = new BarsApiResponse { IsSuccess = false, ErrorResponse = new ErrorResponse() { } };
            _mockBarsService.Setup(s => s.GetBarsData(parameters)).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsDateRange(parameters) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo(expectedData));
            var response = result.Value as BarsApiResponse;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ErrorResponse, Is.Not.Null);
        }
        [Test]
        public async Task GetBarsDateRange_ReturnsBadResult_WhenDataIsNull()
        {
            // Arrange
            var parameters = new BarsDateRangeQueryParameters { };
            BarsApiResponse expectedData = null;
            _mockBarsService.Setup(s => s.GetBarsData(parameters)).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsDateRange(parameters) as BadRequestObjectResult;

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
            var expectedData = new BarsApiResponse { IsSuccess = true, SuccessResponse = new() };
            _mockBarsService.Setup(s => s.GetBarsData(parameters)).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsDateRange(parameters) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expectedData));
            var response = result.Value as BarsApiResponse;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.SuccessResponse, Is.Not.Null);
        }
        [Test]
        public async Task GetBarsTimeBack_ReturnsBadResult_WithExpectedData()
        {
            // Arrange
            var parameters = new BarsTimeBackQueryParameters { };
            var expectedData = new BarsApiResponse { IsSuccess = false, ErrorResponse = new ErrorResponse() { } };
            _mockBarsService.Setup(s => s.GetBarsData(parameters)).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsDateRange(parameters) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo(expectedData));
            var response = result.Value as BarsApiResponse;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ErrorResponse, Is.Not.Null);
        }
        [Test]
        public async Task GetBarsTimeBack_ReturnsBadResult_WhenDataIsNull()
        {
            // Arrange
            var parameters = new BarsTimeBackQueryParameters { };
            BarsApiResponse expectedData = null;
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
            var expectedData = new BarsApiResponse { IsSuccess = true, SuccessResponse = new() };
            _mockBarsService.Setup(s => s.GetBarsData(parameters)).Returns(Task.FromResult(expectedData));

            // Act
            var result = await _controller.GetBarsDateRange(parameters) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expectedData));
            var response = result.Value as BarsApiResponse;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.SuccessResponse, Is.Not.Null);
        }
    }
}
