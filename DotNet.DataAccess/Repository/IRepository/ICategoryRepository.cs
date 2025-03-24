using System;
using System.Linq.Expressions;
using DotNet.Models;

namespace DotNet.DataAccess.Repository.IRepository
{
	public interface ICategoryRepository : IRepository<Category>
	{
        void Update(Category obj);
        // Save all changes
        Task<bool> AnyAsync(Expression<Func<Category, bool>> predicate);
    }
}

