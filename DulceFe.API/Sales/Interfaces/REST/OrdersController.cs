using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Sales.Domain.Repositories;
using DulceFe.API.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace DulceFe.API.Sales.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration.AppDbContext _context;

    public OrdersController(IOrderRepository orderRepository, IUnitOfWork unitOfWork, DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration.AppDbContext context)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderRepository.ListAsync();
        return Ok(orders);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        order.OrderDate = DateTime.UtcNow;
        
        // 1. Save the order
        await _orderRepository.AddAsync(order);
        
        // 2. Synchronize Sales (HU-35, 73)
        try 
        {
             var products = await _context.Products.ToListAsync();
             foreach(var p in products) {
                 if (order.ItemsJson.Contains(p.Title)) {
                     p.Sales++;
                 }
             }
        } catch {}

        // 3. Consume Coupon (HU-32, 74)
        if (!string.IsNullOrEmpty(order.CouponCode)) 
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == order.CouponCode && c.IsActive);
            
            if (coupon != null && coupon.AssignedUserId.HasValue) 
            {
                coupon.IsActive = false; // Consume personal coupon
            }
        }

        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(GetAllOrders), new { id = order.Id }, order);
    }
}
