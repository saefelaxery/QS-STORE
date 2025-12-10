using LaLaStore.Models;
using LaLaStore.Data;
using Microsoft.EntityFrameworkCore;

namespace LaLaStore.Services;

public class OrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(Order order, List<CartItem> cartItems)
    {
        // Calculate total price
        order.TotalPrice = cartItems.Sum(item => item.Subtotal);
        order.OrderDate = DateTime.Now;
        order.Status = "Pending";

        // Add order items
        foreach (var cartItem in cartItems)
        {
            var orderItem = new OrderItem
            {
                ProductId = cartItem.Product.Id,
                ProductName = cartItem.Product.Name,
                Size = cartItem.Size,
                Color = cartItem.Color,
                Quantity = cartItem.Quantity,
                Price = cartItem.Product.Price,
                Subtotal = cartItem.Subtotal
            };
            order.OrderItems.Add(orderItem);
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public List<Order> GetAllOrders()
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .OrderByDescending(o => o.OrderDate)
            .ToList();
    }

    public Order? GetOrderById(int id)
    {
        return _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.Id == id);
    }

    public async Task UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}



