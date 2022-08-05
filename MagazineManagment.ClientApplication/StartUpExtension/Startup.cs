using MagazineManagment.ClientApplication.Models;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.Web.ApiCalls;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCalls;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagazineManagment.ClientApplication.StartUpExtension
{
    public static class Startup
    {
        public static IServiceCollection InjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IProductApiCalls, ProductApiCalls>();
            services.AddTransient<ICategoryApiCalls, CategoryApiCalls>();
            services.AddTransient<IProfileApiCalls, ProfileApiCalls>();
            services.Configure<FetchApiValue>(configuration.GetSection(FetchApiValue.SectionName));
            services.Configure<JwtConfig>(configuration.GetSection("JWTConfig"));
            services.AddControllersWithViews();
            services.AddMemoryCache();
            services.AddSession();
            return services;
        }
    }
}
