using DotNet.DataAccess.Data;
using DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetMVC.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var viewModel = new CategoryIndexViewModel
            {
                Categories = _db.Categories.ToList(),
                CategoryForm = new Category()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicates using AsNoTracking to avoid tracking issues
                var duplicate = await _db.Categories
                    .AsNoTracking()
                    .AnyAsync(c => c.Name.ToLower() == model.CategoryForm.Name.ToLower());

                if (!duplicate)
                {
                    _db.Categories.Add(model.CategoryForm);
                    await _db.SaveChangesAsync();
                    TempData["SuccessMessage"] = model.CategoryForm.Name + " Category added!";
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    // TempData is only available for next render
                    TempData["ErrorMessage"] = "A category with this name already exists";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred -- Entry not valid! Try again";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? categoryId)
        {
            if(categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            Category? obj = _db.Categories.Find(categoryId);
            if(object.Equals(obj, null))
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate name (excluding the current category)
                var duplicate = await _db.Categories
                    .AsNoTracking()
                    .AnyAsync(c => c.CategoryId!=model.CategoryId && c.Name.ToLower() == model.Name.ToLower());
                if (!duplicate)
                {
                    try
                    {
                        // Approach 1: Attach and mark as modified
                        _db.Entry(model).State = EntityState.Modified;

                        // Or Approach 2: Find and update
                        // var existingCategory = await _db.Categories.FindAsync(model.Id);
                        // if (existingCategory != null)
                        // {
                        //     _db.Entry(existingCategory).CurrentValues.SetValues(model);
                        // }

                        await _db.SaveChangesAsync();
                        return RedirectToAction("Index", "Category");
                    }
                    catch (Exception ex)
                    {
                        // Works only inside the form tag, not outside
                        ModelState.AddModelError("", "Error updating record: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "A category with this name already exists");
                }   
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = await _db.Categories.FindAsync(model.CategoryForm.CategoryId);
                if (existingCategory != null)
                {
                    _db.Categories.Remove(existingCategory);
                    await _db.SaveChangesAsync();
                    TempData["SuccessMessage"] = existingCategory.Name + " Category deleted!";
                    return RedirectToAction("Index", "Category");
                }else
                {
                    TempData["ErrorMessage"] = "Couldn't find record to delete";
                }
            }
            return RedirectToAction("Index");
        }
    }
}

