using DulceFe.API.Services.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DulceFe.API.Services.Interfaces.REST;

[ApiController]
[Route("api/v1/inquiries")]
[Produces(MediaTypeNames.Application.Json)]
public class InquiriesController : ControllerBase
{
    private readonly ICateringInquiryQueryService _queryService;
    private readonly ICateringInquiryCommandService _commandService;

    public InquiriesController(ICateringInquiryQueryService queryService, ICateringInquiryCommandService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInquiries()
    {
        var inquiries = await _queryService.Handle();
        return Ok(inquiries);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        try
        {
            await _commandService.Handle(id, status);
            return Ok(new { message = "Status updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
