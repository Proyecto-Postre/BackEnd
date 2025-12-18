using System.Net.Mime;
using DulceFe.API.Catalog.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Interfaces.REST.Resources;
using DulceFe.API.Catalog.Interfaces.REST.Transform;
using DulceFe.API.Shared.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DulceFe.API.Catalog.Interfaces.REST;

[ApiController]
[Route("api/v1/admin/products")]
[Produces(MediaTypeNames.Application.Json)]
public class AdminProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AdminProductsController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductResource resource)
    {
        var product = new Product
        {
            Title = resource.Title,
            Description = resource.Description,
            Price = resource.Price,
            Category = resource.Category,
            Image = resource.Image,
            IsFeatured = resource.IsFeatured
        };
        await _context.Products.AddAsync(product);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductResource resource)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        product.Title = resource.Title;
        product.Description = resource.Description;
        product.Price = resource.Price;
        product.Category = resource.Category;
        product.Image = resource.Image;
        product.IsFeatured = resource.IsFeatured;

        _context.Products.Update(product);
        await _unitOfWork.CompleteAsync();
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }
}
