namespace Chat.Persistence.Services;

public class RedisKeyHelper
{
    public const string Separator = ":";

    public static string[] SplitKey(string key)
    {
        return key.Split(new string[1] { Separator }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string BuildKey(params string[] parts)
    {
        string key = string.Join(Separator, parts);
        return key;
    }
}
