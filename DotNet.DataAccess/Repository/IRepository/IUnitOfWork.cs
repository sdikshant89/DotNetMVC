using System;
namespace DotNet.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
        Task<int> SaveChangesAsync();
    }
}

