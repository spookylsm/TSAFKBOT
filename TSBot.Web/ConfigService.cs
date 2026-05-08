namespace TSBot.Web;

using System.Text.Json;
using global::TSBot.Shared;

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
            var defaultConfig = new BotConfig();
            Save(defaultConfig);
        }
    }

    public BotConfig Get() 
    {
        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<BotConfig>(json) ?? new BotConfig();
    }

    public void Save(BotConfig c) 
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_path, JsonSerializer.Serialize(c, options));
    }
}