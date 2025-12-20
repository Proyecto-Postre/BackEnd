using System.Net.Mime;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Sales.Interfaces.REST;

[ApiController]
[Route("api/v1/admin/analytics")]
[Produces(MediaTypeNames.Application.Json)]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AnalyticsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var userCount = await _context.Users.CountAsync();
        var productCount = await _context.Products.CountAsync();
        var orderCount = await _context.Orders.CountAsync();
        var totalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount);
        
        var topProducts = await _context.Products
            .OrderByDescending(p => p.Sales)
            .Take(5)
            .ToListAsync();
            
        // Calculate recent sales trend (last 7 days) if needed, for now simplified.

        return Ok(new
        {
            Stats = new 
            {
                TotalUsers = userCount,
                TotalProducts = productCount,
                TotalOrders = orderCount,
                TotalRevenue = totalRevenue
            },
            TopProducts = topProducts,
            RecentActivity = new List<object>() // Placeholder for future activity log
        });
    }
}
