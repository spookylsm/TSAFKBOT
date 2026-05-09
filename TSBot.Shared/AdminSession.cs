namespace TSBot.Shared;

public class AdminSession
{
    public bool IsConnected { get; set; } = false;
    public string CurrentTheme { get; set; } = "cyborg"; 
    public List<UserViewModel> Users { get; set; } = new();
    public List<ChannelView> Channels { get; set; } = new();
}