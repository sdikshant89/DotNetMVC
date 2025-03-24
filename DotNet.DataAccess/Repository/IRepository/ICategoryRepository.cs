using System;
using DotNet.Models;

namespace DotNet.DataAccess.Repository.IRepository
{
	public interface ICategoryRepository : IRepository<Category>
	{
        void Update(Category obj);

        // Save all changes
        Task<int> SaveChangesAsync();
    }
}

