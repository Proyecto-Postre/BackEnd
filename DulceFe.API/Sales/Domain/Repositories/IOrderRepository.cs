using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Shared.Domain.Repositories;

namespace DulceFe.API.Sales.Domain.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IEnumerable<Order>> FindByUserIdAsync(int userId);
}
