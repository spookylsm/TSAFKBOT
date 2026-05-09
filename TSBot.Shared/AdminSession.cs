namespace TSBot.Shared;

public class AdminSession
{
    public bool EstaLigado { get; set; } = false;
    public string TemaAtual { get; set; } = "cyborg";
    
    public List<UserViewModel> Users { get; set; } = new();
    public List<ChannelView> Canais { get; set; } = new();
}