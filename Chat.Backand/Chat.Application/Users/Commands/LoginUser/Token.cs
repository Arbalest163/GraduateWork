namespace Chat.Application.Users.Commands.LoginUser;

public class Token
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int Expires { get; set; }
}
