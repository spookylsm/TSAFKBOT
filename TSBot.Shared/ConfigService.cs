namespace TSBot.Shared;

using System.Text.Json;
using TeamSpeak3QueryApi.Net.Specialized;

public class ConfigService 
{
    private readonly string _path = "/app/config/settings.json";

    public ConfigService()
    {
        var dir = Path.GetDirectoryName(_path);
        if (dir != null && !Directory.Exists(dir)) 
        {
            Directory.CreateDirectory(dir);
        }

        if (!File.Exists(_path))
        {
            Save(new BotConfig());
        }
    }

    public BotConfig Get() 
    {
        if (!File.Exists(_path)) return new BotConfig();
        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<BotConfig>(json) ?? new BotConfig();
    }

    public void Save(BotConfig config) 
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_path, JsonSerializer.Serialize(config, options));
    }
    
    public async Task<List<ChannelView>> GetChannelsAsync(BotConfig config)
    {
        using var client = new TeamSpeakClient(config.ServerAddress, config.QueryPort);
        await client.Connect();
        await client.Login(config.QueryUsername, config.QueryPassword);
        await client.UseServer(config.VirtualServerId);
    
        var tsChannels = await client.GetChannels();
        await client.Logout();

        return tsChannels.Select(c => new ChannelView { Id = c.Id, Name = c.Name }).ToList();
    }
    
    public async Task KickClientAsync(BotConfig config, int clid, string reason)
    {
        using var client = new TeamSpeakClient(config.ServerAddress, config.QueryPort);
        await client.Connect();
        await client.Login(config.QueryUsername, config.QueryPassword);
        await client.UseServer(config.VirtualServerId);
    
        await client.KickClient(clid, KickOrigin.Server, reason);
    }

    public async Task BanClientAsync(BotConfig config, int clid, int seconds, string reason)
    {
        using var client = new TeamSpeakClient(config.ServerAddress, config.QueryPort);
        await client.Connect();
        await client.Login(config.QueryUsername, config.QueryPassword);
        await client.UseServer(config.VirtualServerId);

        await client.BanClient(clid, TimeSpan.FromSeconds(seconds), reason);
    }
    
    public async Task<List<UserViewModel>> GetOnlineClientsAsync(BotConfig config)
    {
        var activeUsers = new List<UserViewModel>();

        using var client = new TeamSpeakClient(config.ServerAddress, config.QueryPort);
        await client.Connect();
        await client.Login(config.QueryUsername, config.QueryPassword);
        await client.UseServer(config.VirtualServerId);

        var clients = await client.GetClients();

        foreach (var c in clients)
        {
            // Ignore the bot itself and Query Clients (Type 1)
            if ((int)c.Type == 1) continue;

            var userInfo = await client.GetClientInfo(c.Id);

            activeUsers.Add(new UserViewModel
            {
                Id = c.Id,
                Nickname = c.NickName,
                IdleTime = userInfo.IdleTime.ToString(@"hh\:mm\:ss"),
                Version = userInfo.Version
            });
        }

        await client.Logout();
        return activeUsers;
    }
}