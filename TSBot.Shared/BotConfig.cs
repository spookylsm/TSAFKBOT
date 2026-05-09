namespace TSBot.Shared;

public class BotConfig
{
    public string ServerAddress { get; set; } = "";
    public int QueryPort { get; set; } = 10011;
    public string QueryUsername { get; set; } = "serveradmin";
    public string QueryPassword { get; set; } = "";
    public int VirtualServerId { get; set; } = 1;
    public int AfkChannelId { get; set; }
    public int IdleTimeThresholdMinutes { get; set; } = 15;
}

public class ChannelView
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class UserViewModel
{
    public int Id { get; set; }
    public string Nickname { get; set; } = "";
    public string Version { get; set; } = "";
    public string IdleTime { get; set; } = "";
}