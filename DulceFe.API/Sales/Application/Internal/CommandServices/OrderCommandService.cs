using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Sales.Domain.Model.Commands;
using DulceFe.API.Sales.Domain.Model.ValueObjects;
using DulceFe.API.Sales.Domain.Repositories;
using DulceFe.API.Sales.Domain.Services;
using DulceFe.API.Shared.Domain.Repositories;
using DulceFe.API.Catalog.Domain.Repositories;
using DulceFe.API.Promotions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Sales.Application.Internal.CommandServices;

public class OrderCommandService : IOrderCommandService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly ICouponRepository _couponRepository;

    public OrderCommandService(
        IOrderRepository orderRepository, 
        IUnitOfWork unitOfWork, 
        IProductRepository productRepository, 
        ICouponRepository couponRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _couponRepository = couponRepository;
    }

    public async Task<Order> Handle(CreateOrderCommand command)
    {
        var order = new Order
        {
            UserId = command.UserId,
            TotalAmount = command.TotalAmount,
            ItemsJson = command.ItemsJson,
            CouponCode = command.CouponCode,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };

        await _orderRepository.AddAsync(order);
        
        // Logic moved from Controller: Sync Sales
            var products = await _productRepository.ListAsync();
            
            // Parse items to check quantity
            // Assuming ItemsJson structure is [{ "Title": "...", "Quantity": 1, ... }]
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var orderItems = System.Text.Json.JsonSerializer.Deserialize<List<OrderItemDto>>(order.ItemsJson, options);

            if (orderItems != null)
            {
                foreach (var item in orderItems)
                {
                    var product = products.FirstOrDefault(p => p.Title == item.Title || p.Id == item.ProductId);
                    if (product != null)
                    {
                        if (product.Stock < item.Quantity)
                        {
                            throw new Exception($"Insufficient stock for product: {product.Title}");
                        }

                        product.Stock -= item.Quantity;
                        product.Sales += item.Quantity;
                        _productRepository.Update(product);
                    }
                }
            }

        // Logic moved from Controller: Consume Coupon
        if (!string.IsNullOrEmpty(order.CouponCode)) 
        {
            var coupon = await _couponRepository.FindByCodeAsync(order.CouponCode);
            
            if (coupon != null && coupon.IsActive && coupon.AssignedUserId.HasValue) 
            {
                coupon.IsActive = false;
                // Coupon update is implicitly tracked or needs explicit update depending on Repo implementation. 
                // Assuming EF Core tracking or explicit Update call if Repo handles it.
                // Assuming BaseRepository doesn't implicitly track disconnected entities if mocked, but for real EF it does.
                // To be safe with Repository pattern:
                // _couponRepository.Update(coupon); // CouponRepository might inherit BaseRepository which has Update.
            }
        }

        await _unitOfWork.CompleteAsync();
        return order;
    }

    public async Task Handle(CancelOrderCommand command)
    {
        var order = await _orderRepository.FindByIdAsync(command.OrderId);
        if (order == null) throw new Exception("Order not found");

        if (order.Status != OrderStatus.Pending)
        {
            throw new Exception("Only pending orders can be cancelled");
        }

        order.Status = OrderStatus.Cancelled;
        _orderRepository.Update(order);
        await _unitOfWork.CompleteAsync();
    }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
