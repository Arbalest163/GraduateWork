namespace Chat.Persistence;

public class ChatApiOptions
{
    public int AppId { get; set; }
    public string ServiceKey { get; set; }
    public int LifeTimeAccessToken { get; set; }
    public int LifeTimeRefreshToken { get; set; }
}
