using AutoMapper;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace MagazineManagment.DTO.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Products Mapping
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ProductCategory.CategoryName));


            CreateMap<ProductCreateViewModelNoIFormFile, Product>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Product, ProductPostEditViewModel>();
                
            CreateMap<ProductPostEditViewModel, Product>();

            CreateMap<Product, ProductsAndCategoryInfoViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ProductCategory.CategoryName))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.ProductCategoryId));

            CreateMap<Product, ProductImageOnly>();

            CreateMap<ProductRecordsChanged, ProductsRecordCopyViewModel>()
                .ForMember(dest => dest.ChangesInQunatity, opt => opt.MapFrom(src => src.ProductInStock))
                .ForMember(dest => dest.QuantityBeforeChange, opt => opt.MapFrom(src => src.QunatityBeforeRemoval))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.CreatedOn));

            // Category Mapping
            CreateMap<Category, CategoryViewModel>();

            CreateMap<CategoryCreateViewModel, Category>();

            CreateMap<Category, CategoryNameOnlyViewModel>();

            CreateMap<CategoryCreateViewModel, Category>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<CategoryUpdateViewModel, Category>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
            // Profile Mapping
            CreateMap<IdentityRole, RolesGetAllDetails>()
                .ForMember(dest => dest.RoleNameNormalized, opt => opt.MapFrom(src => src.NormalizedName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityRole, RoleCreateViewModel>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name));
            CreateMap<RoleCreateViewModel, IdentityRole>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName));

            CreateMap<IdentityRole, RoleFindViewModel>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            CreateMap<IdentityRole, ProfileUpdateViewModel>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ProfileUpdateViewModel, IdentityRole>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.RoleName.ToUpper()));

            CreateMap<IdentityRole, RolesGetAllDetails>()
                .ForMember(dest => dest.RoleNameNormalized, opt => opt.MapFrom(src => src.NormalizedName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));

            CreateMap<IdentityUser, UserInRoleViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => true));

            CreateMap<IdentityUser, UserNotInRoleViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false));
        }
    }
}