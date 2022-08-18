using FluentValidation;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.DTO.FluentValidators
{
    public class ProductCreateValidation : AbstractValidator<ProductCreateViewModel>
    {
        private readonly ApplicationDbContext _context;
        public ProductCreateValidation(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.SerialNumber)
                .Must(isSerialNumberExisting).WithMessage("Serial number taken please choose another one");
        }

        private bool isSerialNumberExisting(string serialNumber)
        {
            return !_context.Products.Where(_context => _context.SerialNumber == serialNumber).Any();
        }
    }
}