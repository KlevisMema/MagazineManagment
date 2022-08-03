using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MagazineManagment.BLL.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Create a category
        public async Task<ResponseService<CategoryViewModel>> CreateCategoryAsync(CategoryCreateViewModel category)
        {
            try
            {
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
                    return ResponseService<CategoryViewModel>.NotFound($"Category with id of {id} does not exists");

                return ResponseService<CategoryViewModel>.Ok(_mapper.Map<CategoryViewModel>(category));
            }
            catch (Exception ex)
            {
                return ResponseService<CategoryViewModel>.ExceptioThrow(ex.Message);
            }

        }

        // update a category
        public async Task<ResponseService<CategoryViewModel>> UpdateCategoryAsync(CategoryUpdateViewModel category)
        {
            try
            {
                var findCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

                if (findCategory is null)
                    return ResponseService<CategoryViewModel>.NotFound($"Category with id : {category.Id} doesn't exists!!");

                findCategory.CategoryName = category.CategoryName;
                findCategory.CreatedOn = DateTime.Now;

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
                {
                    return ResponseService<CategoryViewModel>.NotFound($"Category with id : {id} doesn't exists!!");
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

        public async Task<IEnumerable<CategoryNameOnlyViewModel>> GetNamesOnlyCategories()
        {
            var categoryNames = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryNameOnlyViewModel>>(categoryNames);
        }
    }
}