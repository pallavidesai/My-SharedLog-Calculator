using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SharedCalculatorSockets.CalSocketManager
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _connections = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetCalSocketByID(string ID)
        {
            return _connections.FirstOrDefault(p => p.Key == ID).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAllConnections()
        {
            return _connections;
        }

        public string GetIDOfSocket(WebSocket socket)
        {
            return _connections.FirstOrDefault(p => p.Value == socket).Key;
        }

        public async Task AsyncRemoveSocket(string ID)
        {
            _connections.TryRemove(ID, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket Connection Successfully Closed",
                CancellationToken.None);
        }

        private string GetConnectionID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public void AddSocket(WebSocket socket)
        {
            _connections.TryAdd(GetConnectionID(), socket);
        }

    }
    
}
