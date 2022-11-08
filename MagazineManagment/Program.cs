using MagazineManagment.DAL.DataSeeding;
using MagazineManagment.Web.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});



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

app.UseCors(MyAllowSpecificOrigins);

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();