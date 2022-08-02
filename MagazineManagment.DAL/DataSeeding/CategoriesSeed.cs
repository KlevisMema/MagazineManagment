using MagazineManagment.DAL.DataContext;
using MagazineManagment.DAL.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MagazineManagment.DAL.DataSeeding
{
    public class CategoriesSeed
    {
        // seeding category data
        public static async Task SeedCategories(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                _context.Database.EnsureCreated();

                if (!_context.Categories.Any())
                {
                    await _context.Categories.AddRangeAsync(new List<Category>()
                    {
                        new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = "Desktop",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "admin@MagazineManagment.com",
                            IsDeleted = false,
                        },
                        new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = "Laptop",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "admin@MagazineManagment.com",
                            IsDeleted = false,
                        },
                        new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = "Keyboard",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "admin@MagazineManagment.com",
                            IsDeleted = false,
                        },
                        new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = "Mouse",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "admin@MagazineManagment.com",
                            IsDeleted = false,
                        },
                        new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = "Mouse pad",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "admin@MagazineManagment.com",
                            IsDeleted = false,
                        },
                        new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = "Cooling pad",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "admin@MagazineManagment.com",
                            IsDeleted = false,
                        },
                        new Category
                        {
                            Id = Guid.NewGuid(),
                            CategoryName = "USB",
                            CreatedOn = DateTime.Now,
                            CreatedBy = "admin@MagazineManagment.com",
                            IsDeleted = false,
                        },
                    });
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}