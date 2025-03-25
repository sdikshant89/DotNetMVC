using DotNet.DataAccess.Repository.IRepository;
using DotNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryController(IUnitOfWork unitWork)
        {
            unitOfWork = unitWork;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new CategoryIndexViewModel
            {
                Categories = (await unitOfWork.Category.GetAll()).ToList(),
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
                bool duplicate = await unitOfWork.Category.AnyAsync(c => c.Name.ToLower() == model.CategoryForm.Name.ToLower());

                if (!duplicate)
                {
                    unitOfWork.Category.Add(model.CategoryForm);
                    await unitOfWork.SaveChangesAsync();
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

        public async Task<IActionResult> Edit(int? categoryId)
        {
            if(categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            Category? obj = await unitOfWork.Category.Get(c => c.CategoryId == categoryId);
            if(Equals(obj, null))
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
                bool duplicate = await unitOfWork.Category.AnyAsync(c => c.CategoryId!=model.CategoryId && c.Name.ToLower() == model.Name.ToLower());
                if (!duplicate)
                {
                    try
                    {
                        unitOfWork.Category.Update(model);
                        await unitOfWork.SaveChangesAsync();
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
                var existingCategory = await unitOfWork.Category.Get(c => c.CategoryId == model.CategoryForm.CategoryId);
                if (existingCategory != null)
                {
                    unitOfWork.Category.Remove(existingCategory);
                    await unitOfWork.SaveChangesAsync();
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

