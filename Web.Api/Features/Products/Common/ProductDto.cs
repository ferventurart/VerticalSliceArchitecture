namespace Web.Api.Features.Products.Common;

public record ProductDto(
    string Id,
    string Category,
    string Name,
    string? Description,
    decimal Price,
    decimal Cost,
    decimal Profit,
    Uri? ImageUri,
    string Status);
