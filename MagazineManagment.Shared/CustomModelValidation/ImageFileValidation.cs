using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.Shared.CustomModelValidation
{
    public class ImageFileValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? file , ValidationContext context)
        {
            IFormFile formFile = file as IFormFile;
            var image = formFile.FileName;
            if (!image.Contains(".jpg") && !image.Contains(".jepg") && !image.Contains(".png"))
            {
                return new ValidationResult("File format incorrect");
            }
            return ValidationResult.Success;
        }
    }
}
