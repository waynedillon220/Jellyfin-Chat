using MediaBrowser.Controller.Net;
using System;

namespace JellyfinLocalChat
{
    public class ClientConnection
    {
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public IWebSocketConnection Connection { get; set; }
    }
}