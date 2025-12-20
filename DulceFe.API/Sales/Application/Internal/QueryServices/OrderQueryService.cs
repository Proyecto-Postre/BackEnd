using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Sales.Domain.Model.Queries;
using DulceFe.API.Sales.Domain.Repositories;
using DulceFe.API.Sales.Domain.Services;
using Microsoft.EntityFrameworkCore;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace DulceFe.API.Sales.Application.Internal.QueryServices;

public class OrderQueryService : IOrderQueryService
{
    private readonly IOrderRepository _orderRepository;
    private readonly AppDbContext _context; // Needed for complex queries if repo doesn't support them yet

    public OrderQueryService(IOrderRepository orderRepository, AppDbContext context)
    {
        _orderRepository = orderRepository;
        _context = context;
    }

    public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query)
    {
        return await _orderRepository.ListAsync();
    }

    public async Task<Order?> Handle(GetOrderByIdQuery query)
    {
        return await _orderRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Order>> Handle(GetOrdersByUserIdQuery query)
    {
        return await _orderRepository.FindByUserIdAsync(query.UserId);
    }

    public async Task<(IEnumerable<Order>, int)> Handle(GetOrdersByUserIdPagedQuery query)
    {
        var baseQuery = _context.Orders.Where(o => o.UserId == query.UserId);
        
        var totalItems = await baseQuery.CountAsync();
        
        var orders = await baseQuery
            .OrderByDescending(o => o.OrderDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
            
        return (orders, totalItems);
    }
}
