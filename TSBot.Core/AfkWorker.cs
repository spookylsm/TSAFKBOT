namespace TSBot.Core;

using TSBot.Shared;
using TeamSpeak3QueryApi.Net.Specialized;


public class AfkWorker : BackgroundService
{
    private readonly ConfigService _configService;
    private readonly ILogger<AfkWorker> _logger;

    public AfkWorker(ConfigService configService, ILogger<AfkWorker> logger) 
    {
        _configService = configService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AfkWorker started. Monitoring TS3 server...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var config = _configService.Get();
                
                if (!string.IsNullOrEmpty(config.QueryPassword) && config.AfkChannelId > 0)
                {
                    using var client = new TeamSpeakClient(config.ServerAddress, config.QueryPort);
                    await client.Connect();
                    await client.Login(config.QueryUsername, config.QueryPassword);
                    await client.UseServer(config.VirtualServerId);

                    var clientList = await client.GetClients();
                    
                    foreach (var user in clientList)
                    {
                        if ((int)user.Type == 1) continue;

                        var userInfo = await client.GetClientInfo(user.Id);
                        
                        if (userInfo.ChannelId != config.AfkChannelId &&
                            userInfo.IdleTime.TotalMinutes >= config.IdleTimeThresholdMinutes)
                        {
                            _logger.LogInformation($"Moving {user.NickName} to AFK channel...");
                            await client.MoveClient(user.Id, config.AfkChannelId);
                        }
                    }
                    await client.Logout();
                }
            }
            catch (Exception ex) 
            { 
                _logger.LogDebug($"Temporary Worker error: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}