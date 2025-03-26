using System.Linq.Expressions;
using DotNet.DataAccess.Data;
using DotNet.DataAccess.Repository.IRepository;
using DotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet.DataAccess.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
        private ApplicationDbContext _db;
		public ProductRepository(ApplicationDbContext db) : base(db)
		{
            _db = db;
		}

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _db.Products.AsNoTracking().AnyAsync(predicate);
        }
    }
}

