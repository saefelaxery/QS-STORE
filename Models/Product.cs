using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LaLaStore.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    
    [Column(TypeName = "TEXT")]
    public string SizesJson { get; set; } = "[]";
    
    [Column(TypeName = "TEXT")]
    public string ColorsJson { get; set; } = "[]";
    
    [NotMapped]
    [JsonIgnore]
    public List<string> Sizes
    {
        get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(SizesJson) ?? new List<string>();
        set => SizesJson = System.Text.Json.JsonSerializer.Serialize(value);
    }
    
    [NotMapped]
    [JsonIgnore]
    public List<string> Colors
    {
        get => System.Text.Json.JsonSerializer.Deserialize<List<string>>(ColorsJson) ?? new List<string>();
        set => ColorsJson = System.Text.Json.JsonSerializer.Serialize(value);
    }
    
    public bool InStock { get; set; } = true;
    public double? Rating { get; set; }
}

