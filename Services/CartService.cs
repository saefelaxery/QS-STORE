using LaLaStore.Models;
using System.Text.Json;

namespace LaLaStore.Services;

public class CartService
{
    private const string CartSessionKey = "Cart";

    public List<CartItem> GetCartItems(ISession session, ProductService productService)
    {
        var cartJson = session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(cartJson))
            return new List<CartItem>();

        var cartData = JsonSerializer.Deserialize<List<CartItemData>>(cartJson);
        if (cartData == null)
            return new List<CartItem>();

        return cartData.Select(data => new CartItem
        {
            Product = productService.GetProductById(data.ProductId) ?? new Product(),
            Quantity = data.Quantity,
            Size = data.Size,
            Color = data.Color
        }).Where(item => item.Product.Id > 0).ToList();
    }

    public void SaveCartItems(ISession session, List<CartItem> items)
    {
        var cartData = items.Select(item => new CartItemData
        {
            ProductId = item.Product.Id,
            Quantity = item.Quantity,
            Size = item.Size,
            Color = item.Color
        }).ToList();

        var cartJson = JsonSerializer.Serialize(cartData);
        session.SetString(CartSessionKey, cartJson);
    }

    private class CartItemData
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    public void AddToCart(ISession session, ProductService productService, Product product, string size, string color)
    {
        var items = GetCartItems(session, productService);
        var existingItem = items.FirstOrDefault(
            item => item.Product.Id == product.Id && item.Size == size && item.Color == color
        );

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            items.Add(new CartItem
            {
                Product = product,
                Quantity = 1,
                Size = size,
                Color = color
            });
        }

        SaveCartItems(session, items);
    }

    public void RemoveFromCart(ISession session, ProductService productService, int productId, string size, string color)
    {
        var items = GetCartItems(session, productService);
        items.RemoveAll(item => 
            item.Product.Id == productId && item.Size == size && item.Color == color);
        SaveCartItems(session, items);
    }

    public void UpdateQuantity(ISession session, ProductService productService, int productId, string size, string color, int quantity)
    {
        var items = GetCartItems(session, productService);
        var item = items.FirstOrDefault(
            i => i.Product.Id == productId && i.Size == size && i.Color == color
        );

        if (item != null)
        {
            if (quantity <= 0)
                items.Remove(item);
            else
                item.Quantity = quantity;
        }

        SaveCartItems(session, items);
    }

    public void ClearCart(ISession session)
    {
        session.Remove(CartSessionKey);
    }

    public int GetTotalItems(ISession session, ProductService productService)
    {
        return GetCartItems(session, productService).Sum(item => item.Quantity);
    }

    public decimal GetTotalPrice(ISession session, ProductService productService)
    {
        return GetCartItems(session, productService).Sum(item => item.Subtotal);
    }
}

