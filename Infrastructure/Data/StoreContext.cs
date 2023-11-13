using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection; // This is for ApplyConfigurationsFromAssembly in Line 27
using Microsoft.EntityFrameworkCore;
using Core.Entities;


namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options): base(options)
        {
        }    

        public DbSet<Product> Products {get; set;}

        public DbSet<ProductBrand> ProductBrand {get; set;}

        public DbSet<ProductType> ProductType {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
             modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}