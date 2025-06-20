using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNet.Models;
using DotNet.DataAccess.Repository.IRepository;
using DotNet.Models.ViewModels;

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

        public async Task<IActionResult> Index()
        {
            var viewModel = new ProductListingModel {
                Products = await unitOfWork.Product.GetAll(includeProperties: "Category"),
                TotalCount = await unitOfWork.Product.CountAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductListingModel model)
        {
            IEnumerable<Product> productList = await unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
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
