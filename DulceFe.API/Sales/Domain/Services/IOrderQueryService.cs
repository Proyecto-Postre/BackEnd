using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Sales.Domain.Model.Queries;

namespace DulceFe.API.Sales.Domain.Services;

public interface IOrderQueryService
{
    Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query);
    Task<Order?> Handle(GetOrderByIdQuery query);
    Task<IEnumerable<Order>> Handle(GetOrdersByUserIdQuery query);
    // Returning tuple for paged result: (Orders, TotalItems)
    Task<(IEnumerable<Order>, int)> Handle(GetOrdersByUserIdPagedQuery query); 
}
