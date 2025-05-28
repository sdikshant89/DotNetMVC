using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNet.Models;
using DotNet.DataAccess.Repository.IRepository;

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

        // returns webpage inside view directory (same folder name as the controller name (home) and
        // same html page name as the controller function name (index) -- this behaviour is by default)
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> productList = await unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
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
