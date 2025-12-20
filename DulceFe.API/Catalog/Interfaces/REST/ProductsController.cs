using DulceFe.API.Catalog.Domain.Services;
using DulceFe.API.Catalog.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DulceFe.API.Catalog.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProductsController : ControllerBase
{
    private readonly IProductQueryService _productQueryService;

    public ProductsController(IProductQueryService productQueryService)
    {
        _productQueryService = productQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var getAllProductsQuery = new GetAllProductsQuery();
        var products = await _productQueryService.Handle(getAllProductsQuery);
        
        var totalItems = products.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var pagedProducts = products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(new 
        {
            Data = pagedProducts,
            Meta = new 
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = page,
                PageSize = pageSize
            }
        });
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStockProducts([FromQuery] int threshold = 5)
    {
        var query = new GetLowStockProductsQuery(threshold);
        var products = await _productQueryService.Handle(query);
        var resources = products.Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var getProductByIdQuery = new GetProductByIdQuery(id);
        var product = await _productQueryService.Handle(getProductByIdQuery);
        if (product == null) return NotFound();
        var resource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
        return Ok(resource);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetProductsByCategory(string category)
    {
        var getProductsByCategoryQuery = new GetProductsByCategoryQuery(category);
        var products = await _productQueryService.Handle(getProductsByCategoryQuery);
        var resources = products.Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    // Intelligent Search (HU-11, 12)
    [HttpGet("search")]
    public async Task<IActionResult> SearchProducts([FromQuery] string q)
    {
        var getAllProductsQuery = new GetAllProductsQuery();
        var products = await _productQueryService.Handle(getAllProductsQuery);
        
        if (string.IsNullOrEmpty(q)) return Ok(new { count = 0, results = new List<object>() });

        var results = products
            .Where(p => p.Title.Contains(q, StringComparison.OrdinalIgnoreCase) || 
                        p.Description.Contains(q, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var resources = results.Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(new { count = results.Count, results = resources });
    }

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the relative path or full URL depending on needs. 
        // For Render, you might need a proper static file serving setup or a cloud bucket.
        // For now, returning relative path for local development consistency.
        var imageUrl = $"/images/products/{uniqueFileName}";
        return Ok(new { imageUrl });
    }

    // Random Recommendations (HU-14)
    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRandomRecommendations()
    {
        var getAllProductsQuery = new GetAllProductsQuery();
        var products = await _productQueryService.Handle(getAllProductsQuery);
        
        var random = new Random();
        var recommendations = products
            .OrderBy(x => random.Next())
            .Take(3)
            .Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(recommendations);
    }
}
