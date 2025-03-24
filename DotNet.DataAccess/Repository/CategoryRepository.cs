using DotNet.DataAccess.Data;
using DotNet.DataAccess.Repository.IRepository;
using DotNet.Models;

namespace DotNet.DataAccess.Repository
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
        private ApplicationDbContext _db;
		public CategoryRepository(ApplicationDbContext db) : base(db)
		{
            _db = db;
		}

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
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

