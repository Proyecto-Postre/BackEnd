using System.Net.Mime;
using DulceFe.API.Promotions.Interfaces.REST.Resources;
using DulceFe.API.Promotions.Interfaces.REST.Transform;
using DulceFe.API.Shared.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Promotions.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CouponsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CouponsController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponResource resource)
    {
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.Code == resource.Code && c.IsActive);

        if (coupon == null) return NotFound(new { message = "Cupón inválido" });
        if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate < DateTime.UtcNow)
        {
            coupon.IsActive = false;
            await _unitOfWork.CompleteAsync();
            return BadRequest(new { message = "Cupón expirado" });
        }

        return Ok(CouponTransformers.ToResource(coupon));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponResource resource)
    {
        var coupon = CouponTransformers.ToEntity(resource);
        await _context.Coupons.AddAsync(coupon);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(GetCouponById), new { id = coupon.Id }, CouponTransformers.ToResource(coupon));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCouponById(int id)
    {
        var coupon = await _context.Coupons.FindAsync(id);
        if (coupon == null) return NotFound();
        return Ok(CouponTransformers.ToResource(coupon));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCoupons()
    {
        var coupons = await _context.Coupons.ToListAsync();
        return Ok(coupons.Select(CouponTransformers.ToResource));
    }
}
