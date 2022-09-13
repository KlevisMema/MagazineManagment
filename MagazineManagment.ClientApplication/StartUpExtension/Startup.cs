using FluentValidation;
using FluentValidation.AspNetCore;
using FormHelper;
using MagazineManagment.ClientApplication.Models;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.FluentValidators;
using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Web.ApiCalls;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCall.GenericApiCall;
using MagazineManagmet.ApiCalls.ApiCalls;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagazineManagment.ClientApplication.StartUpExtension
{
    public static class Startup
    {
        [Obsolete]
        public static IServiceCollection InjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews()
                .AddFormHelper(options =>
                {
                    options.RedirectDelay = 3000;
                    options.EmbeddedFiles = true;
                })
                .AddFluentValidation();
            services.AddRazorPages();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddAuthentication();

            services.Configure<FetchApiValue>(configuration.GetSection(FetchApiValue.SectionName));
            services.Configure<JwtConfig>(configuration.GetSection("JWTConfig"));

            services.AddTransient<IProductApiCalls, ProductApiCalls>();
            services.AddTransient<ICategoryApiCalls, CategoryApiCalls>();
            services.AddTransient<IProfileApiCalls, ProfileApiCalls>();
            services.AddTransient(typeof(IGenericApi<>), typeof(GenericApi<>));
            services.AddTransient<IValidator<CategoryCreateViewModel>, CategoryCreateValidator>();
            services.AddTransient<IValidator<CategoryViewModel>, CategoryUpdateValidator>();
            services.AddTransient<IValidator<ProductCreateViewModel>, ProductCreateValidation>();

            return services;
        }
    }
}