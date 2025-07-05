namespace Web.Api.Entities;

public sealed class ProductCategory
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; } = [];
    public static string NewId() => $"pc_{Guid.CreateVersion7()}";
}
