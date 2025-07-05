namespace Web.Api.Entities;

public sealed class Customer
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? IdentificationNumber { get; set; }
    public DateOnly? BirthDate { get; set; }
    public required string PhoneNumber { get; set; }
    public required CustomerStatus Status { get; set; }
    public static string NewId() => $"c_{Guid.CreateVersion7()}";
}

public enum CustomerStatus
{
    Active = 0,
    Inactive = 1,
    Suspended = 2
}
