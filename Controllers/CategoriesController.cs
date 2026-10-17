using CwEstate.Catalog.Data;
using CwEstate.Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CwEstate.Catalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(CatalogDbContext db) : ControllerBase
{
    private readonly CatalogDbContext _db = db;

    [HttpGet]
    public async Task<List<Category>> List(CancellationToken ct) =>
        await _db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(ct);

    [HttpPost]
    public async Task<ActionResult<Category>> Create(Category category, CancellationToken ct)
    {
        category.Id = Guid.NewGuid();
        _db.Categories.Add(category);
        await _db.SaveChangesAsync(ct);
        return Created($"/api/categories/{category.Id}", category);
    }
}
