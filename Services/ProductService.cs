using LaLaStore.Models;
using LaLaStore.Data;
using Microsoft.EntityFrameworkCore;

namespace LaLaStore.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Product> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    public Product? GetProductById(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }

    public List<Product> GetProductsByCategory(string? category)
    {
        var allProducts = _context.Products.ToList();
        
        if (string.IsNullOrEmpty(category) || category == "todos" || category == "all")
            return allProducts;
        
        return allProducts
            .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<string> GetCategories()
    {
        return _context.Products
            .Select(p => p.Category)
            .Distinct()
            .ToList();
    }
}
