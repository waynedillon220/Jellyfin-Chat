# Jellyfin Local Chat Plugin WIP

Lightweight local chatbox for Jellyfin where all logged-in users can communicate in real-time. All chats are stored locally on the Jellyfin server.

## Features

- **Real-time messaging** with WebSocket connections
- **Typing indicators** - see when others are typing
- **Live online user list** - view who's currently online
- **Message history** - persistent chat history stored locally
- **Right-click delete** - instant message deletion (replaces with "deleted message")
- **Toggle visibility** - hide/show chat overlay
- **Player integration** - chat disappears with player controls
- **Fully local** - no external services or data transmission

## Installation

### Option 1: One-Click Install (Recommended)

1. In Jellyfin Dashboard, go to **Plugins** → **Repositories**
2. Add this repository URL: `https://raw.githubusercontent.com/waynedillon220/Jellyfin-Chat/main/manifest.json`
3. Go to **Plugins** → **Catalog**
4. Find "Local Chat" and click **Install**
5. Restart your Jellyfin server
6. Enable the plugin in **Dashboard** → **Plugins**

### Option 2: Manual Install

1. Download the latest `JellyfinLocalChat.zip` from [Releases](https://github.com/waynedillon220/Jellyfin-Chat/releases)
2. Extract the ZIP to your Jellyfin server's plugins directory:
   - **Windows**: `C:\ProgramData\Jellyfin\Server\plugins\`
   - **Linux**: `/var/lib/jellyfin/plugins/`
   - **Docker**: `/config/plugins/` (inside container)
3. Restart your Jellyfin server
4. Enable the plugin in **Dashboard** → **Plugins**

## Usage

Once installed and enabled:

1. The chat overlay will automatically appear on all Jellyfin web interface pages
2. Click the chat bubble icon to toggle chat visibility
3. Start typing to send messages to all online users
4. Right-click any message to delete it
5. View the online user list in the chat panel

## Development

### Building from Source

```bash
# Clone the repository
git clone https://github.com/waynedillon220/Jellyfin-Chat.git
cd Jellyfin-Chat

# Build the plugin
./build.bat  # Windows
# or
dotnet build JellyfinLocalChat/JellyfinLocalChat.csproj -c Release -o "build/JellyfinLocalChat"

# Create distribution ZIP
# (ZIP creation steps in build.bat)
```

### Project Structure

```
Jellyfin-Chat/
├── JellyfinLocalChat/          # Main plugin code
│   ├── Plugin.cs               # Plugin entry point & service registration
│   ├── ChatService.cs          # Message storage & management
│   ├── ChatWebSocket.cs        # WebSocket API endpoints
│   ├── ChatMessage.cs          # Message data model
│   └── JellyfinLocalChat.csproj # Project configuration
├── web/                        # Frontend assets
│   ├── inject.html            # Script injection
│   └── chat-overlay.js        # Chat UI overlay
├── manifest.json              # Plugin manifest
├── build.bat                  # Build script
└── README.md                  # This file
```

## Security & Privacy

- **Fully local**: All communication stays within your Jellyfin server
- **No external connections**: No data is sent to external services
- **Local storage**: Messages are stored in JSON files on your server
- **User isolation**: Chat is limited to users logged into your Jellyfin instance

## Compatibility

- **Jellyfin**: 10.9.0+
- **.NET**: 8.0+
- **Platforms**: Windows, Linux, Docker

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

See LICENSE file for details.
