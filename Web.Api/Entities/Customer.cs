namespace Web.Api.Entities;

public sealed class Customer
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? IdentificationNumber { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string PhoneNumber { get; set; }
    public CustomerStatus Status { get; set; }
    public static string NewId() => $"c_{Guid.CreateVersion7()}";
}

public enum CustomerStatus
{
    Active = 0,
    Inactive = 1,
    Suspended = 2,
    Deleted = 3
}
