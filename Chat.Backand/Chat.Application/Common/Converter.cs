using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Chat.Application.Common;

public class Converter
{
    private static string DefaultImage = "DefaultImage.png";
    private static string DirectoryImage = "bin\\Debug\\net7.0\\Images";
    public static byte[] ToBytes(IFormFile? file)
    {
        if (file is not null)
        {
            try
            {
                using var binaryReader = new BinaryReader(file.OpenReadStream());
                return binaryReader.ReadBytes((int)file.Length);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
        return Array.Empty<byte>();
    }

    public static string ToBase64(byte[] bytes)
    {
        if (bytes == null || !bytes.Any())
        {
            return string.Empty;
        }

        return Convert.ToBase64String(bytes);
    }

    public static byte[] ToBytes(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
        {
            return Array.Empty<byte>();
        }

        return Convert.FromBase64String(base64);
    }

    public static string CreateBase64File(string fileName)
    {
        string appDirectory = Directory.GetCurrentDirectory();

        string imagePath = Path.Combine(appDirectory, DirectoryImage, fileName);

        var filePath = File.Exists(imagePath) switch
        {
            true => imagePath,
            _ => Path.Combine(appDirectory, DirectoryImage, DefaultImage)
        };

        var ext = Path.GetExtension(filePath);
        var prefix = GetPrefixBase64(ext);

        var bytes = File.ReadAllBytes(filePath);

        return $"{prefix}{Convert.ToBase64String(bytes)}";
    }

    public static string SaveBase64File(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
        {
            return string.Empty;
        }

        Match match = Regex.Match(base64, @"data:image/(.*);base64,(.*)");
        var extension = match.Groups[1].Value;

        var base64WithoutPrefix = match.Groups[2].Value;
        
        var fileName = $"{Guid.NewGuid}.{extension}";
        var bytes = Convert.FromBase64String(base64WithoutPrefix);

        string appDirectory = Directory.GetCurrentDirectory();

        string imagePath = Path.Combine(appDirectory, DirectoryImage, fileName);

        File.WriteAllBytes(imagePath, bytes);

        return fileName;
    }

    private static string GetPrefixBase64(string extensions)
    {
        return extensions switch
        {
            ".jpg" => "data:image/jpg;base64,",
            ".jpeg" => "data:image/jpeg;base64,",
            ".png" => "data:image/png;base64,",
            _ => throw new NotImplementedException()
        };
    }

    private static string GetExtensionsFromBase64(string base64prefix)
    {
        return base64prefix switch
        {
            "data:image/jpg;base64," => ".jpg",
            "data:image/jpeg;base64," => ".jpeg",
            "data:image/png;base64," => ".png",
            _ => throw new NotImplementedException()
        };
    }
}
