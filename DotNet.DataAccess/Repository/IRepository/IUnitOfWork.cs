using System;
namespace DotNet.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        Task<int> SaveChangesAsync();
    }
}