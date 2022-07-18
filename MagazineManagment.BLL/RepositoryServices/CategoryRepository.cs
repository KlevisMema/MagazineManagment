using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.DataTransferObjects;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MagazineManagment.BLL.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //Create a category
        public async Task<ResponseService<CategoryViewModel>> CreateCategoryAsync(CategoryCreateViewModel category)
        {

            try
            {
                Category Newcategory = new()
                {
                    CategoryName = category.CategoryName,
                    CreatedOn = DateTime.Now,
                    CreatedBy =  "Klevis"
                };

                _context.Categories.Add(Newcategory);
                await _context.SaveChangesAsync();
                return ResponseService<CategoryViewModel>.Ok(Newcategory.AsCategoryDto());
            }
            catch (Exception ex)
            {

                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }

        }

        //Get all categories
        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Select(x => x.AsCategoryDto()).ToListAsync();
        }

        //Get a single category by id
        public async Task<ResponseService<CategoryViewModel>> GetCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return ResponseService<CategoryViewModel>.NotFound($"Category with id of {id} does not exists");

            return ResponseService<CategoryViewModel>.Ok(category.AsCategoryDto());
        }

        // update a category
        public async Task<ResponseService<CategoryViewModel>> UpdateCategoryAsync(CategoryUpdateViewModel category)
        {
            var findCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if (findCategory is null)
                return ResponseService<CategoryViewModel>.NotFound($"Category with id : {category.Id} doesn't exists!!");

            try
            {
                findCategory.CategoryName = category.CategoryName;
                findCategory.CreatedOn = DateTime.Now;


                _context.Categories.Update(findCategory);
                await _context.SaveChangesAsync();
                return ResponseService<CategoryViewModel>.Ok(findCategory.AsCategoryDto());
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }

        }

        //Delete a category by id
        public async Task<ResponseService<CategoryViewModel>> DeleteCategoryAsync(Guid id)
        {
            var findCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (findCategory is null)
            {
                return ResponseService<CategoryViewModel>.NotFound($"Category with id : {id} doesn't exists!!");
            }
            try
            {
                _context.Categories.Remove(findCategory);
                await _context.SaveChangesAsync();
                return ResponseService<CategoryViewModel>.Deleted($"Product with id {id} has been deleted!! ");
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }
        }

        public async Task<IEnumerable<CategoryNameOnlyViewModel>> GetNamesOnlyCategories()
        {
            var categoryNames = await _context.Categories.Select(c => c.AsCategoryNameOnlyDto()).ToListAsync();
            return categoryNames;
        }
    }
}
