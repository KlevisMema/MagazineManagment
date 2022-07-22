using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.Web.ApiCalls;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using IdentityServer4.Services;
using MagazineManagment.BLL.RepositoryServices;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<IProductApiCalls, ProductApiCalls>();
builder.Services.Configure<FetchApiValue>(builder.Configuration.GetSection(FetchApiValue.SectionName));




builder.Services.AddControllersWithViews();

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
