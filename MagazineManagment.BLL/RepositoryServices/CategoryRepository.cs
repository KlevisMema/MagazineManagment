using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using MagazineManagment.BLL.RepositoryServices.GenericService;

namespace MagazineManagment.BLL.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IGenericRepository<CategoryViewModel, Category,Category> _genericCrudCategory;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper,
                                 IGenericRepository<CategoryViewModel, Category, Category> genericCrudCategory)
        {
            _context = context;
            _mapper = mapper;
            _genericCrudCategory = genericCrudCategory;

        }

        //Get user username
        private static string GetUser(HttpContext context)
        {
            var headers = context.Request.Headers["Authorization"].ToString().Remove(0, 7);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(headers);
            var getUsername = token.Claims.Where(t => t.Type == "email").Select(v => v.Value).ToArray();

            return getUsername[0].ToString();
        }

        //Create a category
        public async Task<ResponseService<CategoryViewModel>> CreateCategoryAsync(CategoryCreateViewModel category, HttpContext context)
        {
            try
            {
                // check if the category is inactive
                var categoryExistsv2 = _context.Categories.Any(c => c.CategoryName == category.CategoryName && c.IsDeleted == true);
                if (categoryExistsv2)
                    return ResponseService<CategoryViewModel>.ErrorMsg($"Category {category.CategoryName} is inactive but exists, please give another category name");

                // check if category exits this is to prevent the creation of categories with the same name 
                var categoryExists = _context.Categories.Any(c => c.CategoryName == category.CategoryName);
                if (categoryExists)
                    return ResponseService<CategoryViewModel>.ErrorMsg($"Category {category.CategoryName} exists, please give another category name");

                // Get the username who is creating this category
                category.CreatedBy = GetUser(context);

                return await _genericCrudCategory.Create(_mapper.Map<Category>(category));
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }
        }

        //Get all categories
        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
        }

        //Get a single category by id
        public async Task<ResponseService<CategoryViewModel>> GetCategoryAsync(Guid id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    return ResponseService<CategoryViewModel>.NotFound("Category does not exists");

                return ResponseService<CategoryViewModel>.Ok(_mapper.Map<CategoryViewModel>(category));
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }
        }

        // update a category
        public async Task<ResponseService<CategoryViewModel>> UpdateCategoryAsync(CategoryUpdateViewModel category, HttpContext context)
        {
            try
            {
                var findCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

                if (findCategory is null)
                    return ResponseService<CategoryViewModel>.NotFound($"Category with id : {category.Id} doesn't exists!!");

                /* check if the name  that im giving to this category exits already */
                var categoryExists = false;
                if (findCategory.CategoryName != category.CategoryName)
                    categoryExists = await _context.Categories.AnyAsync(c => c.CategoryName == category.CategoryName);
                if (categoryExists)
                    return ResponseService<CategoryViewModel>.ErrorMsg($"Category {category.CategoryName} exists please give another category");

                category.UpdatedBy = GetUser(context);

                return await _genericCrudCategory.Update(_mapper.Map<Category>(category),findCategory);
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }
        }

        //Delete a category by id
        public async Task<ResponseService<CategoryViewModel>> DeleteCategoryAsync(Guid id)
        {
            try
            {
                var findCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (findCategory is null)
                    return ResponseService<CategoryViewModel>.NotFound($"Category with id : {id} doesn't exists!!");

                findCategory.IsDeleted = true;

                return await _genericCrudCategory.Update(findCategory,findCategory);
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }
        }

        // Get  category names only
        public async Task<IEnumerable<CategoryNameOnlyViewModel>> GetNamesOnlyCategories()
        {
            var categoryNames = await _context.Categories.Where(category => category.IsDeleted == false).ToListAsync();
            return _mapper.Map<List<CategoryNameOnlyViewModel>>(categoryNames);
        }

        //  Activate category
        public async Task<ResponseService<CategoryViewModel>> ActivateCategory(Guid id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category == null)
                    return ResponseService<CategoryViewModel>.NotFound("Category does not exists");

                category.IsDeleted = false;

                _context.Categories.Update(category);
                _context.SaveChanges();

                return ResponseService<CategoryViewModel>.SuccessMessage("Category activated");
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }
        }
    }
}