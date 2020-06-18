using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext :DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
        :base(options)
        {}

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Product>().HasData(new Product()
            {
                Id = 100,
                Name = "TestKhalifa",
                Description = "tttttt",
                PictureUrl= "images/logo.png",
                ProductBrandId = 1,
                ProductTypeId = 1,
                Price = 100
            });
        }
    }
}
