using MediaBrowser.Common;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Plugins;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace JellyfinLocalChat
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages, IServiceRegistrator
    {
        public override string Name => "Local Chat";
        public override Guid Id => Guid.Parse("b3d8b5a2-7c2c-4a5a-9d52-111111111111");

        public Plugin(IApplicationPaths applicationPaths) : base(applicationPaths)
        {
            Instance = this;
        }

        public static Plugin Instance { get; private set; }

        // 👇 THIS injects script into Jellyfin automatically
        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "chat-inject",
                    EmbeddedResourcePath = "JellyfinLocalChat.web.inject.html"
                },
                new PluginPageInfo
                {
                    Name = "chat-overlay.js",
                    EmbeddedResourcePath = "JellyfinLocalChat.web.chat-overlay.js"
                }
            };
        }

        public void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ChatService>(provider => new ChatService(ApplicationPaths.DataPath));
            services.AddScoped<ChatWebSocketService>();
        }
    }

    public class PluginConfiguration : BasePluginConfiguration { }
}