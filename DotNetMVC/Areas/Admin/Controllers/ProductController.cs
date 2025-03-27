using DotNet.DataAccess.Repository.IRepository;
using DotNet.Models;
using DotNet.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork unitWork)
        {
            unitOfWork = unitWork;
        }

        public async Task<IActionResult> Index()
        { 
            var viewModel = new ProductIndexViewModel
            {
                Products = (await unitOfWork.Product.GetAll()).ToList(),
                ProductForm = new Product(),
                CategoryList = (await unitOfWork.Category.GetAll()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                })
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductIndexViewModel model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicates using AsNoTracking to avoid tracking issues
                bool duplicate = await unitOfWork.Product.AnyAsync(c => c.Name.ToLower() == model.ProductForm.Name.ToLower());

                if (!duplicate)
                {
                    unitOfWork.Product.Add(model.ProductForm);
                    await unitOfWork.SaveChangesAsync();
                    TempData["SuccessMessage"] = model.ProductForm.Name + " Product Successfully added!";
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    // TempData is only available for next render
                    TempData["ErrorMessage"] = "A Product of same name exists already";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred -- Entry not valid! Try again";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? productId)
        {
            if(productId == null || productId == 0)
            {
                return NotFound();
            }
            Product? obj = await unitOfWork.Product.Get(c => c.ProductId == productId);
            if(Equals(obj, null))
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                bool duplicate = await unitOfWork.Product.AnyAsync(c => c.ProductId!=model.ProductId && c.Name.ToLower() == model.Name.ToLower());
                if (!duplicate)
                {
                    try
                    {
                        unitOfWork.Product.Update(model);
                        await unitOfWork.SaveChangesAsync();
                        return RedirectToAction("Index", "Product");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error updating record: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "A product of same name already exists");
                }   
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int productId)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await unitOfWork.Product.Get(c => c.ProductId == productId);
                if (existingProduct != null)
                {
                    unitOfWork.Product.Remove(existingProduct);
                    await unitOfWork.SaveChangesAsync();
                    TempData["SuccessMessage"] = existingProduct.Name + " Product deleted!";
                    return RedirectToAction("Index", "Product");
                }else
                {
                    TempData["ErrorMessage"] = "Couldn't find record to delete";
                }
            }
            return RedirectToAction("Index");
        }
    }
}

