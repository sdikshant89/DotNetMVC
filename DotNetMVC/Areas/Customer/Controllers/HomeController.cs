using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNet.Models;
using DotNet.DataAccess.Repository.IRepository;
using DotNet.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotNet.Utility;
using Microsoft.EntityFrameworkCore;

namespace DotNetMVC.Areas.Customer.Controllers;

    [Area("Customer")]
    public class HomeController : Controller
    {
        // ToDo: Modify _Layout.cshtml so that the navbar is only visible in dashboard and user pages not in initial intro, sign in, sign up
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unit)
        {
            _logger = logger;
            unitOfWork = unit;
        }

        public IQueryable<Product> ApplySort(IQueryable<Product> query, SortBy sortBy)
        {
            return sortBy switch
            {
                SortBy.PriceAsc => query.OrderBy(p => p.Price),
                SortBy.PriceDesc => query.OrderByDescending(p => p.Price),
                SortBy.BestSeller => query.OrderByDescending(p => p.Rating * p.RatingNos),
                _ => query
            };
        }


        [HttpGet]
        public async Task<IActionResult> Index(ProductListingModel model)
        {
            var search = model.SearchTerm?.Trim().ToLower();
            var categories = await unitOfWork.Category.GetAll();
            model.Categories = categories.Select(c => new Category
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }).ToList();

            var filteredQuery = unitOfWork.Product
                .GetQueryable(p =>
                    (string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search)) &&
                    (model.SelectedCategories == null || model.SelectedCategories.Count == 0 || model.SelectedCategories.Contains(p.Category.Name)),
                    includeProperties: "Category"
                );

            filteredQuery = ApplySort(filteredQuery, model.SortOption);

            var totalCount = await filteredQuery.CountAsync();

            var products = await filteredQuery
                .Skip((model.PageNo - 1) * model.PerPage)
                .Take(model.PerPage)
                .ToListAsync();

            model.Products = products;
            model.TotalCount = totalCount;

            // Update selected items
            foreach (var item in model.PerPageOptions)
            {
                item.Selected = item.Value == model.PerPage.ToString();
            }
            foreach (var item in model.SortOptions)
            {
                item.Selected = item.Value == ((int)model.SortOption).ToString();
            }

        return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            Product product = await unitOfWork.Product.Get(u=> u.ProductId==id , includeProperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
