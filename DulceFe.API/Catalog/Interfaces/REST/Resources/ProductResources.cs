namespace DulceFe.API.Catalog.Interfaces.REST.Resources;

public record ProductResource(int Id, string Title, string Description, decimal Price, string Category, string Image, bool IsFeatured, int Sales);
public record CreateProductResource(string Title, string Description, decimal Price, string Category, string Image, bool IsFeatured);
