﻿using System.Net.WebSockets;

namespace MarketAssetPriceAPI.Data.Services.ControllerService
{
    public class ClientWebSocketWrapper : IClientWebSocket
    {
        private ClientWebSocket _clientWebSocket;

        public ClientWebSocketWrapper()
        {
            _clientWebSocket = new ClientWebSocket();
        }

        public WebSocketState State => _clientWebSocket.State;

        public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            EnsureWebSocketNotDisposed();
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
            EnsureWebSocketNotDisposed();
            return _clientWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public void Dispose()
        {
            _clientWebSocket?.Dispose();
            _clientWebSocket = null;
        }

        private void EnsureWebSocketNotDisposed()
        {
            if (_clientWebSocket == null)
            {
                _clientWebSocket = new ClientWebSocket();
            }
        }
    }
}
