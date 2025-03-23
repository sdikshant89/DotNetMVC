using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DotNetMVC.Models;

namespace DotNetMVC.Controllers;

public class HomeController : Controller
{
    // ToDo: Modify _Layout.cshtml so that the navbar is only visible in dashboard and user pages not in initial intro, sign in, sign up
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // returns webpage inside view directory (same folder name as the controller name (home) and
        // same html page name as the controller function name (index) -- this behaviour is by default)
        return View();
        // Can override the default behaviour and return another html page/view using this
        // return View("Privacy");
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

