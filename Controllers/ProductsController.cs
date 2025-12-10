using Microsoft.AspNetCore.Mvc;
using LaLaStore.Services;
using LaLaStore.Models;

namespace LaLaStore.Controllers;

public class ProductsController : Controller
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    public IActionResult Index(string? categoria)
    {
        var products = _productService.GetProductsByCategory(categoria);
        ViewBag.Categories = _productService.GetCategories();
        ViewBag.SelectedCategory = categoria ?? "todos";
        return View(products);
    }

    public IActionResult Details(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
}

