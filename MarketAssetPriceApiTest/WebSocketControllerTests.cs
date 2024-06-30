using MarketAssetPriceAPI.Data.Controllers;
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
    internal class WebSocketControllerTests
    {
        private Mock<IWebSocketClientControllerService> _webSocketClientServiceMock;
        private WebSocketController _controller;
        [SetUp]
        public void Setup()
        {
            _webSocketClientServiceMock = new Mock<IWebSocketClientControllerService>();
            _controller = new WebSocketController(_webSocketClientServiceMock.Object);
        }

        [Test]
        public async Task StartStreaming_ShouldReturnOk()
        {
            // Arrange
            _webSocketClientServiceMock.Setup(s => s.Start()).Returns(Task.CompletedTask);
            _webSocketClientServiceMock.Setup(s => s.SendSubscription()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.StartStreaming();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo("Streaming started."));
            _webSocketClientServiceMock.Verify(s => s.Start(), Times.Once);
            _webSocketClientServiceMock.Verify(s => s.SendSubscription(), Times.Once);
        }

        [Test]
        public async Task StopStreaming_ShouldReturnOk()
        {
            // Arrange
            _webSocketClientServiceMock.Setup(s => s.Stop()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.StopStreaming();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo("Streaming stopped."));
            _webSocketClientServiceMock.Verify(s => s.Stop(), Times.Once);
        }
    }
}
