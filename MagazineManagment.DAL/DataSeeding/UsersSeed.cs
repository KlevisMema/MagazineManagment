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
            var test = configuration.GetSection(Users.SectionName).GetChildren().ToList().Select(x => new Users
            {
                UserName = x.GetValue<string>("UserName"),
                Password = x.GetValue<string>("Password"),
            });

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles//
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(RoleName.Admin))
                    await roleManager.CreateAsync(new IdentityRole(RoleName.Admin));
                if (!await roleManager.RoleExistsAsync(RoleName.Employee))
                    await roleManager.CreateAsync(new IdentityRole(RoleName.Employee));

                //Users//
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                foreach (var item in test)
                {
                    var User = await userManager.FindByEmailAsync(item.UserName);

                    if (User == null && item.UserName.Contains("admin"))
                    {
                        var newAdminUser = new IdentityUser()
                        {
                            UserName = item.UserName,
                            Email = item.UserName,
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(newAdminUser, item.Password);
                        await userManager.AddToRoleAsync(newAdminUser, RoleName.Admin);
                    }
                    else if (User == null && item.UserName.Contains("employee"))
                    {
                        var newAdminUser = new IdentityUser()
                        {
                            UserName = item.UserName,
                            Email = item.UserName,
                            EmailConfirmed = true
                        };
                        await userManager.CreateAsync(newAdminUser, item.Password);
                        await userManager.AddToRoleAsync(newAdminUser, RoleName.Employee);
                    }
                }
            }
        }
    }
}
