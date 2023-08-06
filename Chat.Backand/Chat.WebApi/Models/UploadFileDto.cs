using Chat.WebApi.CustomAttributes;

namespace Chat.WebApi.Models;

public class UploadFileDto
{
    [CustomFileExtensions
        (FileExtensions = "jpg,jpeg,bmp,png",
         ErrorMessage = "Please select only Supported Files .png | .jpg | .jpeg | .bmp")]
    public IFormFile? File { get; set; }
}
