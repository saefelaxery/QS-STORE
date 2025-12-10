using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LaLaStore.Models;
using LaLaStore.Services;

namespace LaLaStore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ProductService _productService;

    public HomeController(ILogger<HomeController> logger, ProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public IActionResult Index()
    {
        var featuredProducts = _productService.GetAllProducts().Take(4).ToList();
        ViewBag.Categories = _productService.GetCategories();
        return View(featuredProducts);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
