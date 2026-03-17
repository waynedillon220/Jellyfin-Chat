using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace JellyfinLocalChat
{
    public class ChatService
    {
        private readonly string _filePath;
        public List<ClientConnection> Clients = new();
        public HashSet<string> TypingUsers = new();

        public ChatService(string dataPath)
        {
            _filePath = Path.Combine(dataPath, "chat.json");

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public List<ChatMessage> GetMessages()
        {
            return JsonSerializer.Deserialize<List<ChatMessage>>(
                File.ReadAllText(_filePath));
        }

        public void SaveMessages(List<ChatMessage> messages)
        {
            File.WriteAllText(_filePath,
                JsonSerializer.Serialize(messages, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
        }

        public ChatMessage AddMessage(string user, string text)
        {
            var messages = GetMessages();

            var msg = new ChatMessage
            {
                Username = user,
                Message = text
            };

            messages.Add(msg);
            SaveMessages(messages);

            return msg;
        }

        public void DeleteMessage(Guid id)
        {
            var messages = GetMessages();
            var msg = messages.FirstOrDefault(m => m.Id == id);

            if (msg != null)
            {
                msg.Deleted = true;
                msg.Message = "(deleted message)";
                SaveMessages(messages);
            }
        }

        public List<string> GetOnlineUsers()
        {
            return Clients.Select(c => c.Username).Distinct().ToList();
        }
    }
}