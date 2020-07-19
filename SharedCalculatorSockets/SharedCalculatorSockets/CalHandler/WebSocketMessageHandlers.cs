using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using SharedCalculatorSockets.CalSocketManager;

namespace SharedCalculatorSockets.CalHandler
{
    public class WebSocketMessageHandlers : CalSocketManager.CalSocketHandler
    {
        public WebSocketMessageHandlers(ConnectionManager connections) : base(connections)
        {

        }

        public override async Task OnConnected(WebSocket socket)
        {
            await  base.OnConnected(socket);
            var socketID = Connections.GetIDOfSocket(socket);
            await SendMessageToAll($"{socketID} just joined the party**********");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketID = Connections.GetIDOfSocket(socket);
            var message = $"{socketID} said- {Encoding.ASCII.GetString(buffer, 0, result.Count)}";
            Console.WriteLine(message.ToString());
            await SendMessageToAll (message);
        }
    }
}
