using Microsoft.EntityFrameworkCore;
using MagazineManagment.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;  

namespace MagazineManagment.DAL.DataContext
{
    public  class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)  : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductRecordsChanged> ProductRecordsChangeds { get; set; }
    }
}