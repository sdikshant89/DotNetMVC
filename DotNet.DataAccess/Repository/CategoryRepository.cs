using System.Linq.Expressions;
using DotNet.DataAccess.Data;
using DotNet.DataAccess.Repository.IRepository;
using DotNet.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> AnyAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _db.Categories.AsNoTracking().AnyAsync(predicate);
        }
    }
}

