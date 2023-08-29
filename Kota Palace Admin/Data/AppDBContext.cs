using Kota_Palace_Admin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Kota_Palace_Admin.Controllers;

namespace Kota_Palace_Admin.Data
{
    public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<CartItem>().HasMany(a=>a.Extras).WithOne(e=>e.CartItem);
        }

        public DbSet<Business> Business { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Extras> Extras { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<Cart> Cart { get; set; }
        //public DbSet<Kota_Palace_Admin.Controllers.ApplicationViewModel> ApplicationViewModel { get; set; }

    }
}
