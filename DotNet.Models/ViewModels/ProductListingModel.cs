using System;
namespace DotNet.Models.ViewModels
{
	public class ProductListingModel
	{
        public IEnumerable<Product> Products { get; set; }
        public int TotalCount { get; set; }
        public int PageNo { get; set; } = 1;
        public int PerPage { get; set; } = 8;
    }
}

