using System;
using DotNet.DataAccess.Data;
using DotNet.DataAccess.Repository.IRepository;

namespace DotNet.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
		{
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
		}

        // Unit of Work pattern - You can perform multiple operations and save them all at once
        // Transaction control - All changes are committed together or rolled back together
        // Performance - Batch multiple changes into a single database round-trip
        // Returns total number of rows affected after the operation
        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}

