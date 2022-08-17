using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.Shared.CustomModelValidation
{
    public class ImageFileValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? file, ValidationContext context)
        {
            try
            {
                if (file is not IFormFile formFile)
                    return new ValidationResult("Image is required");

                var image = formFile.FileName;

                if (!image.Contains(".jpg") && !image.Contains(".jpeg") && !image.Contains(".png"))
                    return new ValidationResult("File format incorrect");

                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}
