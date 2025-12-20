using DulceFe.API.Promotions.Domain.Model.Commands;
using DulceFe.API.Promotions.Domain.Model.Queries;
using DulceFe.API.Promotions.Domain.Services;
using DulceFe.API.Promotions.Interfaces.REST.Resources;
using DulceFe.API.Promotions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DulceFe.API.Promotions.Interfaces.REST;

[ApiController]
[Route("api/v1/promotions/coupons")]
[Produces(MediaTypeNames.Application.Json)]
public class CouponsController : ControllerBase
{
    private readonly ICouponCommandService _couponCommandService;
    private readonly ICouponQueryService _couponQueryService;

    public CouponsController(ICouponCommandService couponCommandService, ICouponQueryService couponQueryService)
    {
        _couponCommandService = couponCommandService;
        _couponQueryService = couponQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCoupons()
    {
        var query = new GetAllCouponsQuery();
        var coupons = await _couponQueryService.Handle(query);
        var resources = coupons.Select(CouponResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCouponById(int id)
    {
        var query = new GetCouponByIdQuery(id);
        var coupon = await _couponQueryService.Handle(query);
        if (coupon == null) return NotFound();
        var resource = CouponResourceFromEntityAssembler.ToResourceFromEntity(coupon);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponResource resource)
    {
        var command = CreateCouponCommandFromResourceAssembler.ToCommandFromResource(resource);
        var coupon = await _couponCommandService.Handle(command);
        var createdResource = CouponResourceFromEntityAssembler.ToResourceFromEntity(coupon);
        return CreatedAtAction(nameof(GetCouponById), new { id = createdResource.Id }, createdResource);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCoupon(int id, [FromBody] UpdateCouponResource resource)
    {
        try
        {
            var command = UpdateCouponCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var coupon = await _couponCommandService.Handle(command);
            var updatedResource = CouponResourceFromEntityAssembler.ToResourceFromEntity(coupon);
            return Ok(updatedResource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCoupon(int id)
    {
        try
        {
            var command = new DeleteCouponCommand(id);
            await _couponCommandService.Handle(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
