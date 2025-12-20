using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Sales.Domain.Model.Commands;
using DulceFe.API.Sales.Domain.Model.Queries;
using DulceFe.API.Sales.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DulceFe.API.Sales.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class OrdersController : ControllerBase
{
    private readonly IOrderCommandService _orderCommandService;
    private readonly IOrderQueryService _orderQueryService;

    public OrdersController(IOrderCommandService orderCommandService, IOrderQueryService orderQueryService)
    {
        _orderCommandService = orderCommandService;
        _orderQueryService = orderQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var query = new GetAllOrdersQuery();
        var orders = await _orderQueryService.Handle(query);
        return Ok(orders);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetOrdersByUserIdPagedQuery(userId, page, pageSize);
        var (orders, totalItems) = await _orderQueryService.Handle(query);
        
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return Ok(new
        {
            Data = orders,
            Meta = new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = page,
                PageSize = pageSize
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var order = await _orderCommandService.Handle(command);
        return CreatedAtAction(nameof(GetAllOrders), new { id = order.Id }, order);
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        try
        {
            var command = new CancelOrderCommand(id);
            await _orderCommandService.Handle(command);
            return Ok(new { message = "Order cancelled successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
