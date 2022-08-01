using MagazineManagment.DAL.DataContext;
using MagazineManagment.DAL.Models;
using MagazineManagment.Shared.UsersSeedValues;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MagazineManagment.DAL.DataSeeding
{
    public class SeedData
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


        // seeding users and roles 
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles//
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(RoleName.Admin))
                    await roleManager.CreateAsync(new IdentityRole(RoleName.Admin));
                if (!await roleManager.RoleExistsAsync(RoleName.Employee))
                    await roleManager.CreateAsync(new IdentityRole(RoleName.Employee));

                //Users//

                //Admin
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var _optionsAdmin = serviceScope.ServiceProvider.GetService<IOptions<AdminUser>>();
                string adminUserEmail = _optionsAdmin.Value.UserName;

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new IdentityUser()
                    {
                        UserName = adminUserEmail,
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, _optionsAdmin.Value.Password);
                    await userManager.AddToRoleAsync(newAdminUser, RoleName.Admin);
                }

                // Employee
                var _optionsEmployee = serviceScope.ServiceProvider.GetService<IOptions<EmployeeUser>>();
                string appUserEmail = _optionsEmployee.Value.UserName;
                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new IdentityUser()
                    {
                        UserName = appUserEmail,
                        Email = appUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, _optionsEmployee.Value.Password);
                    await userManager.AddToRoleAsync(newAppUser, RoleName.Employee);
                }
            }
        }
    }
}