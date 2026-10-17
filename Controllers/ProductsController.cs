using CwEstate.Catalog.Data;
using CwEstate.Catalog.Models;
using CwEstate.Catalog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CwEstate.Catalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(CatalogDbContext db, PricingService pricing) : ControllerBase
{
    private readonly CatalogDbContext _db = db;
    private readonly PricingService _pricing = pricing;

    [HttpGet]
    public async Task<ActionResult<List<Product>>> List([FromQuery] Guid? categoryId, CancellationToken ct)
    {
        var q = _db.Products.AsNoTracking();
        if (categoryId is Guid c) q = q.Where(p => p.CategoryId == c);
        return await q.OrderBy(p => p.Name).ToListAsync(ct);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Product>> Get(Guid id, CancellationToken ct)
    {
        var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id, ct);
        return product is null ? NotFound() : product;
    }

    [HttpGet("by-sku/{sku}")]
    public async Task<ActionResult<Product>> GetBySku(string sku, CancellationToken ct)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Sku == sku, ct);
        return product is null ? NotFound() : product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product, CancellationToken ct)
    {
        product.Id = Guid.NewGuid();
        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id:guid}/price")]
    public async Task<IActionResult> ChangePrice(Guid id, [FromBody] decimal price, CancellationToken ct)
    {
        return await _pricing.ChangePriceAsync(id, price, ct) ? NoContent() : NotFound();
    }
}
