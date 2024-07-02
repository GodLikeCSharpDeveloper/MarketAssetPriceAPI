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
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        public WebSocketClientControllerService(ITokenControllerService tokenService, IClientWebSocket clientWebSocket) : base(tokenService)
        {
            _clientWebSocket = clientWebSocket;
            this.tokenService = tokenService;
            InitializeCancellationTokenSource();
        }
        private void InitializeCancellationTokenSource()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        public async Task Start()
        {
            try
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                    InitializeCancellationTokenSource();
                var token = tokenService.GetAccessToken().Result;
                _webSocketUri = new Uri($"wss://platform.fintacharts.com/api/streaming/ws/v1/realtime?token={token}");
                await _clientWebSocket.ConnectAsync(_webSocketUri, CancellationToken.None);
                Console.WriteLine("WebSocket connection started.");
                _ = Task.Run(ReceiveData);
            }
            catch (Exception ex)
            {
                await Stop();
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task SendSubscription()
        {
            try
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
            catch (Exception ex)
            {
                await Stop();
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        private async Task ReceiveData()
        {
            try
            {
                var buffer = new byte[1024 * 4];

                while (_clientWebSocket.State == WebSocketState.Open)
                {
                    var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {message}");
                }
            }
            catch (Exception ex)
            {
                await Stop();
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public async Task Stop()
        {
            try
            {
                _cancellationTokenSource.Cancel();
                if (_clientWebSocket.State == WebSocketState.Open || _clientWebSocket.State == WebSocketState.CloseReceived)
                {
                    await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stopping connection", CancellationToken.None);
                }
            }
            finally
            {
                _clientWebSocket?.Dispose();
                Console.WriteLine("WebSocket connection stopped.");
            }
        }
    }
}
