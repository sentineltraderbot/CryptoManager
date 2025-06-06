using CryptoManager.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CryptoManager.Repository.DatabaseContext
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Exchange> Exchange { get; set; }
        public DbSet<Asset> Asset { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<TokenSale> TokenSale { get; set; }
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var property in builder.Model.GetEntityTypes()
                                        .SelectMany(t => t.GetProperties())
                                        .Where(p => p.ClrType == typeof(decimal)))
            {
                property.SetColumnType("decimal(18, 8)");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\mssqllocaldb;Initial Catalog=CryptoManagerDB;Integrated Security=True;");
        }
    }
}
