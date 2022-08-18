using Microsoft.AspNetCore.Http;
using MagazineManagment.DAL.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using MagazineManagment.Shared.Enums;

namespace MagazineManagment.DAL.Models
{
    public class Product : BaseModel
    {
        public string? SerialNumber { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public CurrencyTypeEnum? CurrencyType { get; set; }
        public string? ProductDescription { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public int? ProductInStock { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public virtual Category? ProductCategory { get; set; }
    }
}