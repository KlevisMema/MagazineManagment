using Microsoft.EntityFrameworkCore;
using MagazineManagment.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MagazineManagment.DAL.DataContext
{
    public  class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)  : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
