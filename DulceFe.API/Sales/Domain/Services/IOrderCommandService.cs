using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Sales.Domain.Model.Commands;

namespace DulceFe.API.Sales.Domain.Services;

public interface IOrderCommandService
{
    Task<Order> Handle(CreateOrderCommand command);
    Task Handle(CancelOrderCommand command);
}
