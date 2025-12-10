using Microsoft.AspNetCore.Mvc;
using LaLaStore.Data;
using Microsoft.EntityFrameworkCore;
using LaLaStore.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaLaStore.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly OrderService _orderService;

    public AdminController(ApplicationDbContext context, OrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }

    public IActionResult Database()
    {
        var products = _context.Products.ToList();
        return View(products);
    }

    public IActionResult Orders()
    {
        var orders = _orderService.GetAllOrders();
        return View(orders);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order != null)
        {
            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "تم حذف الطلب بنجاح";
        }
        else
        {
            TempData["ErrorMessage"] = "الطلب غير موجود";
        }

        return RedirectToAction(nameof(Orders));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAllOrders()
    {
        var allOrderItems = _context.OrderItems.ToList();
        var allOrders = _context.Orders.ToList();

        _context.OrderItems.RemoveRange(allOrderItems);
        _context.Orders.RemoveRange(allOrders);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"تم حذف جميع الطلبات ({allOrders.Count} طلب) بنجاح";
        return RedirectToAction(nameof(Orders));
    }
}

