using DulceFe.API.Catalog.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Catalog.Infrastructure.Persistence.EFC.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> FindByQueryAsync(string query)
    {
        return await Context.Set<Product>()
            .Where(p => p.Title.Contains(query) || p.Description.Contains(query))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> FindByCategoryAsync(string category)
    {
        return await Context.Set<Product>()
            .Where(p => p.Category == category)
            .ToListAsync();
    }
}
