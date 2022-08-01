using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.Web.ApiCalls;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCalls;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddTransient<IProductApiCalls, ProductApiCalls>();
builder.Services.AddTransient<ICategoryApiCalls, CategoryApiCalls>();
builder.Services.AddTransient<IProfileApiCalls, ProfileApiCalls>();
builder.Services.Configure<FetchApiValue>(builder.Configuration.GetSection(FetchApiValue.SectionName));
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddSession();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
