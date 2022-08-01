using MagazineManagment.BLL.RepositoryServices;
using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.Services;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.Shared.UsersSeedValues;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagazineManagment.Web.Extensions
{
    public static class StartupExtension
    {
        public static IServiceCollection InjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.Configure<AdminUser>(configuration.GetSection(AdminUser.SectionName));
            services.Configure<EmployeeUser>(configuration.GetSection(EmployeeUser.SectionName));
            return services;
        }
    }
}
