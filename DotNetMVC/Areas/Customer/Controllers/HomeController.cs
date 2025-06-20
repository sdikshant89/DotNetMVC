using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNet.Models;
using DotNet.DataAccess.Repository.IRepository;
using DotNet.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        [HttpGet]
        public async Task<IActionResult> Index(ProductListingModel model)
        {
            var query = await unitOfWork.Product.GetAll(
                filter: p =>
                    (string.IsNullOrEmpty(model.SearchTerm) || p.Name.Contains(model.SearchTerm)) &&
                    (model.SelectedCategories == null || model.SelectedCategories.Contains(p.Category.Name)),
                includeProperties: "Category",
                skip: (model.PageNo - 1) * model.PerPage,
                take: model.PerPage
            );

            var totalCount = await unitOfWork.Product.CountAsync(p =>
                (string.IsNullOrEmpty(model.SearchTerm) || p.Name.Contains(model.SearchTerm)) &&
                (model.SelectedCategories == null || model.SelectedCategories.Contains(p.Category.Name))
            );
            foreach (var item in model.PerPageOptions)
            {
                item.Selected = item.Value == model.PerPage.ToString();
            }
            model.Products = query;
            model.TotalCount = totalCount;

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
