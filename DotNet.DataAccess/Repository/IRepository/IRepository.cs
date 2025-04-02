using System;
using System.Linq.Expressions;

namespace DotNet.DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
        // T - any generic model class

        // Fetch Methods
        Task<IEnumerable<T>> GetAll(string? includeProperties = null);
        Task<T> Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

		// Addition
		void Add(T entity);

        //Deletion
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}

