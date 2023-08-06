using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CustomFileExtensionsAttribute : ValidationAttribute
    {
        public string FileExtensions { get; set; } = "png,jpg,jpeg,bmp";

        public override bool IsValid(object? value)
        {
            IFormFile? file = value as FormFile;
            bool isValid = true;

            var allowedExtensions = FileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (file != null)
            {
                var fileName = file.FileName;

                isValid = allowedExtensions.Any(y => fileName.EndsWith(y));
            }

            return isValid;
        }
    }
}
