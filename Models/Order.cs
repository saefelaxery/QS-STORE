using System.ComponentModel.DataAnnotations;

namespace LaLaStore.Models;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = "CashOnDelivery";
    
    [MaxLength(4)]
    public string? CardLast4 { get; set; }
    
    [MaxLength(100)]
    public string? CardHolderName { get; set; }
    
    [MaxLength(10)]
    public string? CardExpiry { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";
    
    public List<OrderItem> OrderItems { get; set; } = new();
}



