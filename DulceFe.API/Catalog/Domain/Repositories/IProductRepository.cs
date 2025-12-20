using DulceFe.API.Catalog.Domain.Model.Aggregates;
using DulceFe.API.Shared.Domain.Repositories;

namespace DulceFe.API.Catalog.Domain.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<IEnumerable<Product>> FindByQueryAsync(string query);
    Task<IEnumerable<Product>> FindByCategoryAsync(string category);
    Task<IEnumerable<Product>> FindByStockLessThanAsync(int stockThreshold);
}
