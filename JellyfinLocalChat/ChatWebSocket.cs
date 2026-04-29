using MediaBrowser.Controller.Net;
using ServiceStack;
using ServiceStack.Web;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace JellyfinLocalChat
{
    [Route("/api/LocalChat/ws", "GET")]
    public class ChatWebSocket : IReturnVoid { }

    public class ChatWebSocketService : BaseApiService
    {
        private static ChatService _chatService;

        public ChatWebSocketService(ChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task Get(ChatWebSocket request)
        {
            var socket = await Request.AcceptWebSocketAsync();
            var user = RequestContext.User?.Name ?? "Unknown";

            var client = new ClientConnection
            {
                Username = user,
                Socket = socket
            };

            _chatService.Clients.Add(client);

            // SEND HISTORY
            var history = _chatService.GetMessages();
            await Send(socket, new { type = "history", messages = history });

            await BroadcastUsers();

            var buffer = new byte[2048];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var doc = JsonDocument.Parse(json);

                var type = doc.RootElement.GetProperty("type").GetString();

                if (type == "message")
                {
                    var text = doc.RootElement.GetProperty("text").GetString();

                    var msg = _chatService.AddMessage(user, text);

                    await Broadcast(new
                    {
                        type = "message",
                        id = msg.Id,
                        user = user,
                        text = msg.Message
                    });
                }

                if (type == "typing")
                {
                    _chatService.TypingUsers.Add(user);
                    await Broadcast(new { type = "typing", user });
                }

                if (type == "stopTyping")
                {
                    _chatService.TypingUsers.Remove(user);
                }

                if (type == "delete")
                {
                    var id = doc.RootElement.GetProperty("id").GetGuid();
                    _chatService.DeleteMessage(id);

                    await Broadcast(new { type = "delete", id });
                }
            }

            _chatService.Clients.Remove(client);
            await BroadcastUsers();
        }

        private async Task Broadcast(object obj)
        {
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

            foreach (var c in _chatService.Clients)
            {
                if (c.Socket.State == WebSocketState.Open)
                {
                    await c.Socket.SendAsync(data,
                        WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private async Task Send(WebSocket socket, object obj)
        {
            var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
            await socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task BroadcastUsers()
        {
            await Broadcast(new
            {
                type = "users",
                users = _chatService.GetOnlineUsers()
            });
        }
    }
}