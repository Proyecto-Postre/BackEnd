using System.Net.Mime;
using DulceFe.API.Shared.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using DulceFe.API.Social.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Social.Interfaces.REST;

[ApiController]
[Route("api/v1/admin/testimonials")]
[Produces(MediaTypeNames.Application.Json)]
public class AdminTestimonialsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AdminTestimonialsController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpPatch("{id}/pin")]
    public async Task<IActionResult> PinTestimonial(int id)
    {
        var testimonial = await _context.Testimonials.FindAsync(id);
        if (testimonial == null) return NotFound();
        
        testimonial.IsPinned = !testimonial.IsPinned;
        _context.Testimonials.Update(testimonial);
        await _unitOfWork.CompleteAsync();
        return Ok(testimonial);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestimonial(int id)
    {
        var testimonial = await _context.Testimonials.FindAsync(id);
        if (testimonial == null) return NotFound();

        _context.Testimonials.Remove(testimonial);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }
}
