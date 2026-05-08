namespace TSBot.Core;

using TeamSpeak3QueryApi.Net.Specialized;
using TSBot.Shared;

public class AfkWorker : BackgroundService
{
    private const string ConfigPath = "/app/config/settings.json";
    private readonly ILogger<AfkWorker> _logger;

    public AfkWorker(ILogger<AfkWorker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!File.Exists(ConfigPath)) goto Wait;

                var config = System.Text.Json.JsonSerializer.Deserialize<BotConfig>(File.ReadAllText(ConfigPath));
                if (config == null || string.IsNullOrEmpty(config.QueryPassword)) goto Wait;

                using (var client = new TeamSpeakClient(config.ServerAddress, config.QueryPort))
                {
                    await client.Connect();
                    await client.Login(config.QueryUsername, config.QueryPassword);
                    await client.UseServer(config.VirtualServerId);

                    var clientList = await client.GetClients();
                    foreach (var user in clientList)
                    {
                        var userInfo = await client.GetClientInfo(user.Id);
                        
                        // Ignora o próprio bot e Query Clients (Type 1)
                        if ((int)user.Type == 1) continue;

                        if (userInfo.ChannelId != config.AfkChannelId &&
                            userInfo.IdleTime.TotalMinutes >= config.IdleTimeThresholdMinutes)
                        {
                            _logger.LogInformation($"A mover {user.NickName} para o canal AFK...");
                            await client.MoveClient(user.Id, config.AfkChannelId);
                        }
                    }

                    await client.Logout();
                }
            }
            catch (Exception ex) { _logger.LogError($"Error: {ex.Message}"); }

            Wait:
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}