using CwEstate.Catalog.Data;
using CwEstate.Catalog.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CwEstate.Catalog.Services;

public class PricingService(CatalogDbContext db, IPublishEndpoint publishEndpoint)
{
    private readonly CatalogDbContext _db = db;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task<bool> ChangePriceAsync(Guid productId, decimal newPrice, CancellationToken ct)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId, ct);
        if (product is null) return false;
        var old = product.Price;
        product.Price = newPrice;
        _db.Products.Update(product);
        await _db.SaveChangesAsync(ct);
        await _publishEndpoint.Publish(new ProductPriceChanged(product.Id, product.Sku, old, newPrice, DateTime.UtcNow), ct);
        return true;
    }
}
