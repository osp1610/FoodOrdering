using LoginPage.Models;
using Microsoft.EntityFrameworkCore;
using Admin.Models;

namespace LoginPage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderItem> Orders { get; set; }
    }
}
