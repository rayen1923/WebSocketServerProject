using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using WebSocketServerProject.Models.Domaine;

namespace WebSocketServerProject.MidlleWare
{
    public class WebSocketServerMidlleWare
    {
        private readonly WebSocketServerConnectionManger _connectionManger;

        public WebSocketServerMidlleWare(WebSocketServerConnectionManger connectionManager)
        {
            _connectionManger = connectionManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket websocket =await context.WebSockets.AcceptWebSocketAsync();
                string conID = _connectionManger.AddSocket(websocket);
                await SendMessageAsync(websocket, conID);
                Console.WriteLine("Socket connected...");
                await RecieveMessage(websocket, async (result, message) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        Console.WriteLine("Message received..");
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("WebSocket closed.");
                    }
                });
            }
        }

        private async Task SendMessageAsync(WebSocket websocket, string conID)
        {
            var message = $" ID : {conID}";
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            await websocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task RecieveMessage(WebSocket ws,Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];
            while (ws.State == WebSocketState.Open)
            {
                var res = await ws.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(res, buffer);
            }
        }
    }
}
