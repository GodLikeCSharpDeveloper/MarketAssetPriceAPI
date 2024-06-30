﻿using System.Net.WebSockets;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public class ClientWebSocketWrapper : IClientWebSocket
    {
        private readonly ClientWebSocket _clientWebSocket;

        public ClientWebSocketWrapper()
        {
            _clientWebSocket = new ClientWebSocket();
        }

        public WebSocketState State => _clientWebSocket.State;

        public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            return _clientWebSocket.ConnectAsync(uri, cancellationToken);
        }

        public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            return _clientWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return _clientWebSocket.ReceiveAsync(buffer, cancellationToken);
        }

        public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            return _clientWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public void Dispose()
        {
            _clientWebSocket.Dispose();
        }
    }

}
