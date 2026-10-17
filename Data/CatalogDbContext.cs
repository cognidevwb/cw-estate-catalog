using CwEstate.Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace CwEstate.Catalog.Data;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasIndex(p => p.Sku).IsUnique();
    }
}
