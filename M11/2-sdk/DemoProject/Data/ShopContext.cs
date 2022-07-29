using DemoProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
            
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Order>().ToTable("Order");
        }
    }
}