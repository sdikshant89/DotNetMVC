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
        private readonly IWebHostEnvironment webEnv;
        public ProductController(IUnitOfWork unitWork, IWebHostEnvironment webHostEnvironment)
        {
            unitOfWork = unitWork;
            webEnv = webHostEnvironment;
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
                    string wwwRootPath = webEnv.WebRootPath;
                    if (imageFile != null)
                    {
                        model.ProductForm.ImageUrl = saveImageAndReturnPath(imageFile);
                    }
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
            var vm = new ProductIndexViewModel
            {
                ProductForm = obj,
                CategoryList = (await unitOfWork.Category.GetAll()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                })
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductIndexViewModel model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                bool duplicate = await unitOfWork.Product.AnyAsync(c => c.ProductId!=model.ProductForm.ProductId && c.Name.ToLower() == model.ProductForm.Name.ToLower());
                if (!duplicate)
                {
                    try
                    {
                        if (imageFile != null)
                        {
                            if (!string.IsNullOrEmpty(model.ProductForm.ImageUrl))
                            {
                                // Delete the previous image
                                var oldImagePath = Path.Combine(webEnv.WebRootPath, model.ProductForm.ImageUrl.Replace("\\", "/").TrimStart('/'));
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }    
                            model.ProductForm.ImageUrl = saveImageAndReturnPath(imageFile);
                        }
                        unitOfWork.Product.Update(model.ProductForm);
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

        public string saveImageAndReturnPath(IFormFile imageFile)
        {
            string wwwRootPath = webEnv.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string productImagePath = Path.Combine(wwwRootPath, @"images/Products");
            using (var fileStream = new FileStream(Path.Combine(productImagePath, fileName), FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }
            return @"\images\Products\" + fileName;
        }
    }
}

