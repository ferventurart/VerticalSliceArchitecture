using FluentValidation;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;

namespace Web.Api.Features.Customers;

public static class CreateCustomer
{
    public record CreateCustomerRequest(
        string FirstName,
        string LastName,
        string Email,
        string? IdentificationNumber,
        DateOnly? BirthDate,
        string PhoneNumber);


    public sealed class Validator : AbstractValidator<CreateCustomerRequest>
    {
        public Validator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(r => r.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(300);

            RuleFor(r => r.IdentificationNumber)
                .Matches(@"^\d{8}-\d{1}$")
                .MaximumLength(10);

            RuleFor(x => x.BirthDate)
               .Must(date => date != default)
               .WithMessage("Debes enviar una fecha válida.");

            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\d{4}-\d{4}$")
                .MaximumLength(9);
        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("customers", Handler).WithTags("Customers");
        }
    }

    public static async Task<IResult> Handler(
        CreateCustomerRequest request,
        ApplicationDbContext context,
        IValidator<CreateCustomerRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);

        var customer = new Customer
        {
            Id = Customer.NewId(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            IdentificationNumber = request.IdentificationNumber,
            BirthDate = request.BirthDate,
            Status = CustomerStatus.Active
        };

        context.Customers.Add(customer);

        await context.SaveChangesAsync();

        return Results.Created($"/customers/{customer.Id}", customer.ToDto());
    }
}
