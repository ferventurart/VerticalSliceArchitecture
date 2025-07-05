namespace Web.Api.Entities;

public sealed class Product
{
    public required string Id { get; set; }
    public required string ProductCategoryId { get; set; }
    public ProductCategory? ProductCategory { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public required decimal Cost { get; set; }
    public decimal Profit => Price - Cost;
    public Uri? ImageUri { get; set; }
    public ProductStatus Status { get; set; }
    public static string NewId() => $"p_{Guid.CreateVersion7()}";
}

public enum ProductStatus
{
    Discontinued = 0,
    Active = 1
}
