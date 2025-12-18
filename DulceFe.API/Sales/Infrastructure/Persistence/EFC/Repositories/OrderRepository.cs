using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Sales.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Sales.Infrastructure.Persistence.EFC.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> FindByUserIdAsync(int userId)
    {
        return await Context.Set<Order>()
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }
}
