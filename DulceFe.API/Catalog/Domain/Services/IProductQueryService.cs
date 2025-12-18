using DulceFe.API.Catalog.Domain.Model.Aggregates;

namespace DulceFe.API.Catalog.Domain.Services;

public interface IProductQueryService
{
    Task<IEnumerable<Product>> Handle(GetAllProductsQuery query);
    Task<Product?> Handle(GetProductByIdQuery query);
    Task<IEnumerable<Product>> Handle(GetProductsByCategoryQuery query);
}

public record GetAllProductsQuery();
public record GetProductByIdQuery(int Id);
public record GetProductsByCategoryQuery(string Category);
