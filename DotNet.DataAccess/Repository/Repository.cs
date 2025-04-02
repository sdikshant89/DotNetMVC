using System.Linq.Expressions;
using DotNet.DataAccess.Data;
using DotNet.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DotNet.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext db)
		{
            _db = db;
            // this would initialise dbSet to any entities set of objects
            // eg. _db.Categories = dbSet
            this.dbSet = _db.Set<T>();
		}

        public async Task<IEnumerable<T>> GetAll(string? includeProperties = null)
        {
            // Include properties would be the name of nested objects to be included, case sensitive
            IQueryable<T> query = dbSet;
            try
            {
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return await query.ToListAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Entity of type {typeof(T).Name} not found. Details: {ex.Message}");
            }
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            try
            {
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return await query.FirstAsync();
            }
            catch (InvalidOperationException ex)
            {
                // Catching only this particular type of error
                // Other errors like database connection timeout etc are handled elsewhere in the application
                // Todo Implement the handling of the other global exceptions
                throw new Exception($"Entity of type {typeof(T).Name} not found. Details: {ex.Message}");
            }
        }


        // Methods below don't perform any db changes, they just stage changes to the object model
        // So its alright to not use async await
        // Cause after using we're anyways going to call SaveChangesAsync
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}

