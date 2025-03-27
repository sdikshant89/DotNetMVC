using DotNet.Models;
using Microsoft.EntityFrameworkCore;
namespace DotNet.DataAccess.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

        // Commands for mac migration (run inside the main solution root directory)
        // dotnet tool install --global dotnet-e
        // dotnet ef migrations add {any_random_name} --startup-project ./DotNetMVC --project ./DotNet.DataAccess
        // dotnet ef database update --startup-project ./DotNetMVC --project ./DotNet.DataAccess

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Category>().HasData(
					new Category { CategoryId = 1, Name = "Electronics", DisplayOrder = 1 },
                    new Category { CategoryId = 2, Name = "Toys", DisplayOrder = 2 },
					new Category { CategoryId = 3, Name = "Appliances", DisplayOrder = 3 }
				);

            // modelBuilder.Entity<Product>().Property(p => p.ImageUrl).IsRequired(false); -- using ? nullable reference type in the entity
            modelBuilder.Entity<Product>().HasData(
                    new Product { ProductId = 1, Name = "IPad Pro", Price = 1200M, Description = "10th Gen, 13 Inch", CategoryId = 1 },
                    new Product { ProductId = 2, Name = "Fidget Spinner", Price = 15M, Description = "Stress bbye", CategoryId = 2 },
                    new Product { ProductId = 3, Name = "Refrigerator", Price = 3000M, Description = "Cooling Like never before", CategoryId = 3 }
                );
        }
    }
}