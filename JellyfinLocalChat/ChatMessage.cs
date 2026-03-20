using System;

namespace JellyfinLocalChat
{
    public class ChatMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string Message { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}