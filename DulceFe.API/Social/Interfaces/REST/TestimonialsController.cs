using DulceFe.API.Social.Domain.Model.Aggregates;
using DulceFe.API.Social.Domain.Repositories;
using DulceFe.API.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DulceFe.API.Social.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class TestimonialsController : ControllerBase
{
    private readonly ITestimonialRepository _testimonialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TestimonialsController(ITestimonialRepository testimonialRepository, IUnitOfWork unitOfWork)
    {
        _testimonialRepository = testimonialRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTestimonials()
    {
        var testimonials = await _testimonialRepository.ListAsync();
        return Ok(testimonials);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTestimonial([FromBody] Testimonial testimonial)
    {
        await _testimonialRepository.AddAsync(testimonial);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(GetAllTestimonials), new { id = testimonial.Id }, testimonial);
    }
}
