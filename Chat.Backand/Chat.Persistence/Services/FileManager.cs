using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Chat.Persistence.Services;

public class FileManager : IFileManager
{
    private const string DefaultImage = "DefaultImage.png";
    private const string DirectoryImage = "Images";
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileManager(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string CreateBase64File(string fileName)
    {
        string appDirectory = Directory.GetCurrentDirectory();

        string imagePath = Path.Combine(appDirectory, fileName);

        var filePath = File.Exists(imagePath) switch
        {
            true => imagePath,
            _ => Path.Combine(appDirectory, DefaultImage)
        };

        var ext = Path.GetExtension(fileName);
        var prefix = GetPrefixBase64(ext);

        var bytes = File.ReadAllBytes(filePath);

        return $"{prefix}{Convert.ToBase64String(bytes)}";
    }

    public string CreateDownloadUrl(string source)
    {
        var baseUrl = _webHostEnvironment.WebRootPath;
       
        string appDirectory = Directory.GetCurrentDirectory();

        string imagePath = Path.Combine(appDirectory, source);

        var filePath = File.Exists(imagePath) switch
        {
            true => imagePath,
            _ => Path.Combine(appDirectory, DefaultImage)
        };

        string downloadUrl = Path.Combine(baseUrl, filePath);

        return downloadUrl;
    }

    private string GetPrefixBase64(string extensions)
    {
        return extensions switch
        {
            ".jpg" => "data:image/jpg;base64,",
            ".jpeg" => "data:image/jpeg;base64,",
            ".png" => "data:image/png;base64,",
        };
    }
}
