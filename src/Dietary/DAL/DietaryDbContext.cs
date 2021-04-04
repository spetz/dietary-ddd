using System.Reflection;
using Dietary.Models;
using Microsoft.EntityFrameworkCore;

namespace Dietary.DAL
{
    public class DietaryDbContext : DbContext
    {
        public DietaryDbContext(DbContextOptions<DietaryDbContext> options) : base(options)
        {
        }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOrderGroup> CustomerOrderGroups { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
        }
    }
}