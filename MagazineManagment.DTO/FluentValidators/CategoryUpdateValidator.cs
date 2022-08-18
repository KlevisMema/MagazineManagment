using FluentValidation;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.DTO.FluentValidators
{
    public class CategoryUpdateValidator : AbstractValidator<CategoryViewModel>
    {
        public CategoryUpdateValidator()
        {
            RuleFor(c=>c.Id).NotEmpty();

            RuleFor(c => c.CategoryName).NotEmpty().WithMessage("Please ensure that you have entered a category name")
                .Length(2, 10).WithMessage("Please ensure to enter appropriate length of characters")
                .Matches("^[a-zA-Z ]*$").WithMessage("Please ensure that the category name doesn't contain special characters ex:(-/?><) etc., or numbers.");
        }
    }
}