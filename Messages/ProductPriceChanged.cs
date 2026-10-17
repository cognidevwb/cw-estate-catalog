namespace CwEstate.Catalog.Messages;

/// Raised after a product's price is updated. Consumed by the orders service
/// to keep its price snapshot current.
public record ProductPriceChanged(Guid ProductId, string Sku, decimal OldPrice, decimal NewPrice, DateTime ChangedAt);
