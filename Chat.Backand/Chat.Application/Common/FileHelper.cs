namespace Chat.Application.Common;

public class FileHelper
{
    private static string DefaultAvatarFileName = "DefaultAvatar.jpg";
    public static string SaveAvatar(string fileName, byte[] fileData)
    {
        try
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            string avatarsFolderPath = Path.Combine(appPath, "avatars");

            if (!Directory.Exists(avatarsFolderPath))
            {
                Directory.CreateDirectory(avatarsFolderPath);
            }

            string filePath = Path.Combine(avatarsFolderPath, fileName);

            File.WriteAllBytes(filePath, fileData);

            return filePath;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static string GetDefaultAvatar()
    {
        return DefaultAvatarFileName;
    }
}
