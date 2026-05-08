namespace TSBot.Shared;

public class BotConfig
{
    public string ServerAddress { get; set; } = "127.0.0.1";
    public int QueryPort { get; set; } = 10011;
    public string QueryUsername { get; set; } = "serveradmin";
    public string QueryPassword { get; set; } = "";
    public int VirtualServerId { get; set; } = 1;
    public int AfkChannelId { get; set; } = 0;
    public int IdleTimeThresholdMinutes { get; set; } = 15;
}