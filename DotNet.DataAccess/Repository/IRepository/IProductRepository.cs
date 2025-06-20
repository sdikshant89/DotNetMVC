using System.Linq.Expressions;
using DotNet.Models;

namespace DotNet.DataAccess.Repository.IRepository
{
	public interface IProductRepository : IRepository<Product>
	{
        void Update(Product obj);
        // Save all changes
        Task<bool> AnyAsync(Expression<Func<Product, bool>> predicate);
        Task<int> CountAsync(Expression<Func<Product, bool>>? predicate = null);
    }
}