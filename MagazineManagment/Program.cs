using MagazineManagment.DAL.DataSeeding;
using MagazineManagment.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.InjectServices(builder.Configuration);

var app = builder.Build();

await CategoriesSeed.SeedCategories(app);
await UsersSeed.SeedUsersAndRolesAsync(app, builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();