# cw-estate-catalog

The product catalog of the estate: products, categories and prices.

- ASP.NET Core 9 Web API, EF Core (SQLite), MassTransit over RabbitMQ.
- Owns the `Products` and `Categories` tables.
- Publishes `ProductPriceChanged` whenever a price is updated.
- Called by `cw-estate-orders`, `cw-estate-inventory`, `cw-estate-storefront` and `cw-estate-admin`.

Run: `dotnet run` (listens on http://localhost:5001).
