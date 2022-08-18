using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace MagazineManagment.BLL.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        //Get user username
        private static string GetUser(HttpContext context)
        {
            var headers = context.Request.Headers["Authorization"].ToString().Remove(0, 7);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(headers);
            var getUsername = token.Claims.Where(t => t.Type == "email").Select(v => v.Value).ToArray();

            return getUsername[0].ToString();
        }

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Create a category
        public async Task<ResponseService<CategoryViewModel>> CreateCategoryAsync(CategoryCreateViewModel category, HttpContext context)
        {
            try
            {
                category.CreatedBy = GetUser(context);
                Category Newcategory = _mapper.Map<Category>(category);

                _context.Categories.Add(Newcategory);
                await _context.SaveChangesAsync();
                return ResponseService<CategoryViewModel>.Ok(_mapper.Map<CategoryViewModel>(Newcategory));
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
        public async Task<ResponseService<CategoryViewModel>> UpdateCategoryAsync(CategoryUpdateViewModel category,HttpContext context)
        {
            try
            {
                var findCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

                if (findCategory is null)
                    return ResponseService<CategoryViewModel>.NotFound($"Category with id : {category.Id} doesn't exists!!");

                findCategory.CategoryName = category.CategoryName;
                findCategory.CreatedOn = DateTime.Now;
                findCategory.CreatedBy = GetUser(context);

                _context.Categories.Update(findCategory);
                await _context.SaveChangesAsync();

                return ResponseService<CategoryViewModel>.Ok(_mapper.Map<CategoryViewModel>(findCategory));
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

                var findProductWithThisCategory = _context.Products.Where(c => c.ProductCategoryId.Equals(id)).ToList();

                foreach (var item in findProductWithThisCategory)
                {
                    item.ProductCategoryId = null;
                    _context.Products.Update(item);
                }

                _context.Categories.Remove(findCategory);
                await _context.SaveChangesAsync();
                return ResponseService<CategoryViewModel>.Deleted($"Product with id {id} has been deleted!! ");
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }
        }

        // Get  category names only
        public async Task<IEnumerable<CategoryNameOnlyViewModel>> GetNamesOnlyCategories()
        {
            var categoryNames = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryNameOnlyViewModel>>(categoryNames);
        }
    }
}