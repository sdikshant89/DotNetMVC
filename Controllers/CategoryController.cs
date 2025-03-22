using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetMVC.Data;
using DotNetMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetMVC.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var viewModel = new CategoryIndexViewModel
            {
                Categories = _db.Categories.ToList(),
                CategoryForm = new Category()
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryIndexViewModel model)
        {
            var categories = _db.Categories.ToList();

            if (string.Equals(string.Empty, model.CategoryForm.Name) &&
                !categories.Any(c => c.Name.ToLower() == model.CategoryForm.Name.ToLower()))
            {
                _db.Categories.Add(model.CategoryForm);
                await _db.SaveChangesAsync();
            }
            //Todo Check how to send custom return message and show in screen
            return RedirectToAction("Index", "Category");
        }
    }
}

