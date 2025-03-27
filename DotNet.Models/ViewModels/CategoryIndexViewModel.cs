using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DotNet.Models.ViewModels
{
	public class CategoryIndexViewModel
	{
        [ValidateNever]
        public List<Category> Categories { get; set; }
        public Category CategoryForm { get; set; }
    }
}

