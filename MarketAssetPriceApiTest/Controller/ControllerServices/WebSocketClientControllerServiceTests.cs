using MarketAssetPriceAPI.Data.Services.ControllerService;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MarketAssetPriceApiTest.Controller.ControllerServices
{
    [TestFixture]
    public class WebSocketClientControllerServiceTests
    {
        private Mock<IClientWebSocket> _mockClientWebSocket;
        private Mock<ITokenControllerService> _mockTokenService;
        private WebSocketClientControllerService _service;

        [SetUp]
        public void Setup()
        {
            _mockClientWebSocket = new Mock<IClientWebSocket>();
            _mockTokenService = new Mock<ITokenControllerService>();
            _service = new WebSocketClientControllerService(_mockTokenService.Object, _mockClientWebSocket.Object);
        }

        [Test]
        public async Task Start_ShouldConnectWebSocket()
        {
            // Arrange
            _mockTokenService.Setup(t => t.GetAccessToken()).ReturnsAsync("test_token");

            // Act
            await _service.Start();

            // Assert
            _mockClientWebSocket.Verify(c => c.ConnectAsync(It.Is<Uri>(u => u.ToString() == "wss://platform.fintacharts.com/api/streaming/ws/v1/realtime?token=test_token"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task SendSubscription_ShouldSendSubscriptionMessage()
        {
            // Arrange
            var subscription = new
            {
                type = "l1-subscription",
                id = "1",
                instrumentId = "ad9e5345-4c3b-41fc-9437-1d253f62db52",
                provider = "simulation",
                subscribe = true,
                kinds = new[] { "ask", "bid", "last" }
            };

            var message = JsonConvert.SerializeObject(subscription);
            var bytes = Encoding.UTF8.GetBytes(message);

            // Act
            await _service.SendSubscription();

            // Assert
            _mockClientWebSocket.Verify(c => c.SendAsync(It.Is<ArraySegment<byte>>(b => b.Array.SequenceEqual(bytes)), WebSocketMessageType.Text, true, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Stop_ShouldCloseWebSocket()
        {
            // Arrange

            // Act
            await _service.Stop();

            // Assert
            _mockClientWebSocket.Verify(c => c.Dispose(), Times.Once);
        }
    }

}
