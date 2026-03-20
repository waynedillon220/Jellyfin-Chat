public IEnumerable<PluginPageInfo> GetPages()
{
    return new[]
    {
        new PluginPageInfo
        {
            Name = "chat-overlay.js",
            EmbeddedResourcePath = "JellyfinLocalChat.web.chat-overlay.js"
        }
    };
}