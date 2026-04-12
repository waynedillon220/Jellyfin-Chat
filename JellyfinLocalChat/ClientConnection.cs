using System.Net.WebSockets;

namespace JellyfinLocalChat
{
    public class ClientConnection
    {
        public string Username { get; set; }
        public WebSocket Socket { get; set; }
    }
}