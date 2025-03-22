using System;
using DotNetMVC.Models;
using Microsoft.EntityFrameworkCore;
namespace DotNetMVC.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{}

		public DbSet<Category> Categories { get; set; }
	}
}