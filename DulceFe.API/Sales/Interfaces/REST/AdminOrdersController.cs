using System.Net.Mime;
using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Shared.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DulceFe.API.Sales.Domain.Model.ValueObjects;

namespace DulceFe.API.Sales.Interfaces.REST;

[ApiController]
[Route("api/v1/admin/orders")]
[Produces(MediaTypeNames.Application.Json)]
public class AdminOrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AdminOrdersController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _context.Orders.ToListAsync();
        return Ok(orders);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();
        
        if (Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
        {
            order.Status = orderStatus;
            _context.Orders.Update(order);
            await _unitOfWork.CompleteAsync();
            return Ok(order);
        }
        return BadRequest("Invalid status");
    }
}
