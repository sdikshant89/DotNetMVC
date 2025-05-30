using DotNet.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNet.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{}

        // Commands for mac migration (run inside the main solution root directory)
        // dotnet tool install --global dotnet-e
        // dotnet ef migrations add {any_random_name} --startup-project ./DotNetMVC --project ./DotNet.DataAccess
        // dotnet ef database update --startup-project ./DotNetMVC --project ./DotNet.DataAccess

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Category>().HasData(
					new Category { CategoryId = 1, Name = "Electronics", DisplayOrder = 1 },
                    new Category { CategoryId = 2, Name = "Toys", DisplayOrder = 2 },
					new Category { CategoryId = 3, Name = "Appliances", DisplayOrder = 3 }
				);

            // modelBuilder.Entity<Product>().Property(p => p.ImageUrl).IsRequired(false); -- using ? nullable reference type in the entity
            modelBuilder.Entity<Product>().HasData(
                    new Product
                    {
                        ProductId = 1,
                        Name = "IPad Pro",
                        Price = 1200M,
                        Brand = "Apple",
                        Rating = 4.6M,
                        RatingNos = 155,
                        Description = "10th Gen, 13 Inch",
                        CategoryId = 1
                    },
                    new Product
                    {
                        ProductId = 2,
                        Name = "Fidget Spinner",
                        Price = 15M,
                        Brand = "ToyComp",
                        Rating = 3.8M,
                        RatingNos = 2000,
                        Description = "Stress bbye",
                        CategoryId = 2
                    },
                    new Product
                    {
                        ProductId = 3,
                        Name = "Refrigerator",
                        Price = 3000M,
                        Brand = "Samsung",
                        Rating = 4.8M,
                        RatingNos = 450,
                        Description = "Cooling Like never before",
                        CategoryId = 3
                    },
                    new Product
                    {
                        ProductId = 4,
                        Name = "Gaming Wireless mouse",
                        Price = 120M,
                        Brand = "Logitech",
                        Rating = 4.8M,
                        RatingNos = 4500,
                        Description = "Best DPI ranging from 2000 to 16000",
                        CategoryId = 1
                    }
                );
        }
    }
}