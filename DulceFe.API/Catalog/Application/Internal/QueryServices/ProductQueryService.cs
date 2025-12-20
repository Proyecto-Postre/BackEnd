using DulceFe.API.Catalog.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Domain.Repositories;
using DulceFe.API.Catalog.Domain.Services;

namespace DulceFe.API.Catalog.Application.Internal.QueryServices;

public class ProductQueryService : IProductQueryService
{
    private readonly IProductRepository _productRepository;

    public ProductQueryService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery query)
    {
        return await _productRepository.ListAsync();
    }

    public async Task<Product?> Handle(GetProductByIdQuery query)
    {
        return await _productRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsByCategoryQuery query)
    {
        return await _productRepository.FindByCategoryAsync(query.Category);
    }

    public async Task<IEnumerable<Product>> Handle(GetLowStockProductsQuery query)
    {
        return await _productRepository.FindByStockLessThanAsync(query.Threshold);
    }
}
