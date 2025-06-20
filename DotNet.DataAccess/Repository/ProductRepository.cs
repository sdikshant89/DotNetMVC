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

        public async Task<IEnumerable<Product>> GetAll(
            Expression<Func<Product, bool>>? filter = null,
            string? includeProperties = null,
            int? skip = null,
            int? take = null)
        {
            IQueryable<Product> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (skip.HasValue)
                query = query.Skip(skip.Value);
            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _db.Products.AsNoTracking().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<Product, bool>>? predicate = null)
        {
            return predicate != null
                ? await _db.Products.CountAsync(predicate)
                : await _db.Products.CountAsync();
        }

        public IQueryable<Product> GetQueryable(
            Expression<Func<Product, bool>>? filter = null,
            string? includeProperties = null)
        {
            IQueryable<Product> query = _db.Products;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query;
        }

    }
}

