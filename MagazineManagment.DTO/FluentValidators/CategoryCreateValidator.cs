using FluentValidation;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.DTO.FluentValidators
{
    internal class CategoryCreateValidator : AbstractValidator<CategoryCreateViewModel>
    {
        public CategoryCreateValidator()
        {
            RuleFor(c => c.CategoryName)
                .NotEmpty().WithMessage("Please ensure that you have entered a category name")
                .Length(2, 10).WithMessage("Please ensure to enter appropriate lenth of characters")
                .Matches("^[a-zA-Z ]*$").WithMessage("Please ensure that the category name doesn't contain special characters ex:(-/?><) etc.");
        }
    }
}