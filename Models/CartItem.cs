namespace LaLaStore.Models;

public class CartItem
{
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    
    public decimal Subtotal => Product.Price * Quantity;
}

