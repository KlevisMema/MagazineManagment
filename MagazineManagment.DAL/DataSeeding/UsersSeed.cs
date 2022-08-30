using MagazineManagment.Shared.UsersSeedValues;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MagazineManagment.DAL.DataSeeding
{
    public class UsersSeed
    {
        // seeding users and roles
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            //Roles//
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(RoleName.Admin))
                await roleManager.CreateAsync(new IdentityRole(RoleName.Admin));
            if (!await roleManager.RoleExistsAsync(RoleName.Employee))
                await roleManager.CreateAsync(new IdentityRole(RoleName.Employee));

            //Users//
            var getUsers = configuration.GetSection(Users.SectionName).Get<Users[]>();

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            foreach (var item in getUsers)
            {
                var User = await userManager.FindByEmailAsync(item.UserName);

                if (User == null)
                {
                    var newUser = new IdentityUser()
                    {
                        UserName = item.UserName,
                        Email = item.UserName,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(newUser, item.Password);

                    foreach(var role in item.Roles)
                    {
                        await userManager.AddToRoleAsync(newUser, role);
                    }

                }
            }
        }
    }
}