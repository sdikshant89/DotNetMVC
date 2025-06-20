using System;
using DotNet.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNet.Models.ViewModels
{
	public class ProductListingModel
	{
        public IEnumerable<Product> Products { get; set; }

        public string? SearchTerm { get; set; }
        public List<string>? SelectedCategories { get; set; }

        public int PageNo { get; set; } = 1;
        public int TotalCount { get; set; }

        public SortBy SortOption { get; set; } = SortBy.None;
        public List<SelectListItem> SortOptions => new()
        {
            new SelectListItem("Sort", ((int)SortBy.None).ToString()),
            new SelectListItem("Price: Asc", ((int)SortBy.PriceAsc).ToString()),
            new SelectListItem("Price: Desc", ((int)SortBy.PriceDesc).ToString()),
            new SelectListItem("Bestseller", ((int)SortBy.BestSeller).ToString())
        };

        public int PerPage { get; set; } = 8;
        public List<SelectListItem> PerPageOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "4", Value = "4" },
            new SelectListItem { Text = "8", Value = "8" },
            new SelectListItem { Text = "12", Value = "12" }
        };
    }
}

