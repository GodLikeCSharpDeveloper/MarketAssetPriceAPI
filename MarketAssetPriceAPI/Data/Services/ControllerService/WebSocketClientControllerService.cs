using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public class WebSocketClientControllerService : AuthorizedControllerService, IWebSocketClientControllerService
    {
        private readonly IClientWebSocket _clientWebSocket;
        private Uri _webSocketUri;
        private readonly ITokenControllerService tokenService;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        public WebSocketClientControllerService(ITokenControllerService tokenService, IClientWebSocket clientWebSocket) : base(tokenService)
        {
            _clientWebSocket = clientWebSocket;
            this.tokenService = tokenService;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        public async Task Start()
        {
            var token = tokenService.GetAccessToken().Result;
            _webSocketUri = new Uri($"wss://platform.fintacharts.com/api/streaming/ws/v1/realtime?token={token}");
            await _clientWebSocket.ConnectAsync(_webSocketUri, CancellationToken.None);
            Console.WriteLine("WebSocket connection started.");
            _ = Task.Run(ReceiveData);
        }

        public async Task SendSubscription()
        {
            var subscription = new
            {
                type = "l1-subscription",
                id = "1",
                instrumentId = "ad9e5345-4c3b-41fc-9437-1d253f62db52",
                provider = "simulation",
                subscribe = true,
                kinds = new[] { "ask", "bid", "last" }
            };

            var message = JsonSerializer.Serialize(subscription);
            var bytes = Encoding.UTF8.GetBytes(message);

            await _clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("Subscription message sent.");
        }

        private async Task ReceiveData()
        {
            var buffer = new byte[1024 * 4];

            while (_clientWebSocket.State == WebSocketState.Open)
            {
                var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close || _cancellationToken.IsCancellationRequested)
                {
                    await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    Console.WriteLine("WebSocket connection closed.");
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {message}");
                }
            }
        }

        public async Task Stop()
        {
            _cancellationTokenSource.Cancel();
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stopping connection", CancellationToken.None);
            Console.WriteLine("WebSocket connection stopped.");
            _clientWebSocket.Dispose();
        }
    }
}
