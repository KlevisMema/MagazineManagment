using MagazineManagment.DAL.DataSeeding;
using MagazineManagment.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InjectServices(builder.Configuration);

var app = builder.Build();



await UsersSeed.SeedUsersAndRolesAsync(app,builder.Configuration);
await CategoriesSeed.SeedCategories(app);

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