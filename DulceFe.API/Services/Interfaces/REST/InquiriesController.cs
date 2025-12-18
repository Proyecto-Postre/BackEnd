using System.Net.Mime;
using DulceFe.API.Services.Interfaces.REST.Resources;
using DulceFe.API.Services.Interfaces.REST.Transform;
using DulceFe.API.Shared.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Services.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class InquiriesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public InquiriesController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("catering")]
    public async Task<IActionResult> CreateCateringInquiry([FromBody] CreateCateringInquiryResource resource)
    {
        var inquiry = ServiceTransformers.ToEntity(resource);
        await _context.CateringInquiries.AddAsync(inquiry);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(GetCateringInquiryById), new { id = inquiry.Id }, ServiceTransformers.ToResource(inquiry));
    }

    [HttpGet("catering/{id}")]
    public async Task<IActionResult> GetCateringInquiryById(int id)
    {
        var inquiry = await _context.CateringInquiries.FindAsync(id);
        if (inquiry == null) return NotFound();
        return Ok(ServiceTransformers.ToResource(inquiry));
    }

    [HttpPost("contact")]
    public async Task<IActionResult> CreateContactMessage([FromBody] CreateContactMessageResource resource)
    {
        var message = ServiceTransformers.ToEntity(resource);
        await _context.ContactMessages.AddAsync(message);
        await _unitOfWork.CompleteAsync();
        return Ok(ServiceTransformers.ToResource(message));
    }

    [HttpPost("workshops")]
    public async Task<IActionResult> SubscribeToWorkshop([FromBody] CreateWorkshopSubscriptionResource resource)
    {
        var subscription = ServiceTransformers.ToEntity(resource);
        await _context.WorkshopSubscriptions.AddAsync(subscription);
        await _unitOfWork.CompleteAsync();
        return Ok(ServiceTransformers.ToResource(subscription));
    }

    [HttpGet("catering")]
    public async Task<IActionResult> GetAllCateringInquiries()
    {
        var inquiries = await _context.CateringInquiries.ToListAsync();
        var resources = inquiries.Select(ServiceTransformers.ToResource);
        return Ok(resources);
    }
}
