namespace DulceFe.API.Catalog.Domain.Model.Aggregates;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty; // Simulating category name as in db.json for now
    public string Image { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public int Sales { get; set; }
}
