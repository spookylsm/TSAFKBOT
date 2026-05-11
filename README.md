# TSBot - TeamSpeak 3 AFK Bot

TSBot is a bot for TeamSpeak 3 servers that monitors idle users and automatically moves them to a configured AFK channel. It includes a Blazor web interface for configuration and management.

## Features

- Continuous monitoring of users on the TeamSpeak 3 server
- Automatic movement of idle users to the AFK channel
- Web interface for parameter configuration
- Persistent configuration in JSON files
- Docker support for easy deployment

## Prerequisites

- .NET 10.0 SDK
- TeamSpeak 3 server with Query API enabled
- TS3 server admin credentials

## Installation and Execution

### Using Docker (Recommended)

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd Bot
   ```

2. Configure the `bot_data/settings.json` file with your TS3 credentials.

3. Run with Docker Compose:
   ```bash
   docker-compose up -d
   ```

The application will be available at `http://localhost:5000`.

### Docker Compose Example using Docker Hub

If you have the image published on Docker Hub, use the following `docker-compose.yml`:

```yaml
version: '3.8'
services:
  tsbot:
    image: spookylsm2/ts-afk-bot:latest
    container_name: ts_afk_bot
    ports:
      - "5000:8080"
    volumes:
      - ./bot_data:/app/config
    restart: always
```

### Manual Build

1. Restore dependencies:
   ```bash
   dotnet restore
   ```

2. Run the project:
   ```bash
   dotnet run --project TSBot.Web
   ```

## Configuration

Edit the `bot_data/settings.json` file:

```json
{
  "ServerAddress": "your-ts3-server.com",
  "QueryPort": 10011,
  "QueryUsername": "serveradmin",
  "QueryPassword": "your-password",
  "VirtualServerId": 1,
  "AfkChannelId": 123,
  "IdleTimeThresholdMinutes": 15
}
```

- `ServerAddress`: TS3 server address
- `QueryPort`: Query API port (default: 10011)
- `QueryUsername`: Query login user (usually "serveradmin")
- `QueryPassword`: Query user password
- `VirtualServerId`: Virtual server ID (default: 1)
- `AfkChannelId`: AFK channel ID
- `IdleTimeThresholdMinutes`: Idle time in minutes to move the user

## How to Get the AFK Channel ID

1. In the TeamSpeak 3 client, right-click the AFK channel
2. Select "Edit Channel"
3. The ID is in the "General" tab as "Channel ID"

## Project Structure

- `TSBot.Core`: Main bot logic and workers
- `TSBot.Shared`: Shared configurations and models
- `TSBot.Web`: Blazor web interface for configuration

## Contributing

Contributions are welcome! Open an issue or pull request.

## License

This project is under the MIT license.
