using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MarketAssetPriceAPI.Data.Services
{
    public class WebSocketClientService : AuthorizedService
    {
        private readonly ClientWebSocket _clientWebSocket;
        private Uri _webSocketUri;
        private readonly TokenService tokenService;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        public WebSocketClientService(TokenService tokenService) : base(tokenService)
        {
            _clientWebSocket = new ClientWebSocket();
            this.tokenService = tokenService;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        public async Task StartAsync()
        {
            var token = tokenService.GetAccessTokenAsync().Result;
            _webSocketUri = new Uri($"wss://platform.fintacharts.com/api/streaming/ws/v1/realtime?token={token}");
            await _clientWebSocket.ConnectAsync(_webSocketUri, CancellationToken.None);
            Console.WriteLine("WebSocket connection started.");
            _ = Task.Run(async () => await ReceiveDataAsync());
        }

        public async Task SendSubscriptionAsync()
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

        private async Task ReceiveDataAsync()
        {
            var buffer = new byte[1024 * 4];

            while (_clientWebSocket.State == WebSocketState.Open)
            {
                var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close|| _cancellationToken.IsCancellationRequested)
                {
                    await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    Console.WriteLine("WebSocket connection closed.");
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {message}");
                    // Process message
                }
            }
        }

        public async Task StopAsync()
        {
            _cancellationTokenSource.Cancel();
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stopping connection", CancellationToken.None);
            Console.WriteLine("WebSocket connection stopped.");
            _clientWebSocket.Dispose();
        }
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _clientWebSocket?.Dispose();
        }
    }
}
