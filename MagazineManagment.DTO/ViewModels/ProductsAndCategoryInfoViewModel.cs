﻿using MagazineManagment.Shared.Enums;

namespace MagazineManagment.DTO.ViewModels
{
    public class ProductsAndCategoryInfoViewModel
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public string? ProductDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string Image { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public CurrencyTypeEnum? CurrencyType { get; set; }
    }
}