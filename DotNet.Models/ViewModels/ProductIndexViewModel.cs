using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace DotNet.Models.ViewModels
{
	public class ProductIndexViewModel
	{
        [ValidateNever]
        public List<Product> Products { get; set; }
        public Product ProductForm { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}