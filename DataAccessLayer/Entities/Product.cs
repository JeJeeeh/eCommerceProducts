using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.entities;

public class Product
{
    [Key]
    public Guid ProductID { get; set; }
    public string ProductName { get; set; }
    public string Category { get; set; }
    public string? UnitPrice { get; set; }
    public string? QuantityInStock { get; set; }
    
}