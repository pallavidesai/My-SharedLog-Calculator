﻿ using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedCalculator.WebSocketConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if(Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Count() <= 2)
            {
                Process.Start("SharedCalculator.WebSocketConsole.exe");
            }

            StartCalWebSockets().GetAwaiter().GetResult();
        }

        public static async Task StartCalWebSockets()
        {
            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri("ws://localhost:10000/ws"), CancellationToken.None);
            Console.WriteLine($"Web Socket Connection Established @{DateTime.UtcNow:F}");
            var send = Task.Run(async () =>
            {
                string message;
                while((message = Console.ReadLine()) != null & message != string.Empty)
                {
                    var bytes = Encoding.ASCII.GetBytes(message);
                    await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true,
                        CancellationToken.None);
                }

                await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            });

            var receive = ReceiveAsync(client);
            await Task.WhenAll(send, receive);
        }

        public static async Task ReceiveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];
            while(true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));
                if(result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
            }
        }


    }
}
