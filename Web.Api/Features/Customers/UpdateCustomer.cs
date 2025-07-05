using FluentValidation;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.Customers.Common;

namespace Web.Api.Features.Customers;

public static class UpdateCustomer
{
    public record UpdateCustomerRequest(
       string FirstName,
       string LastName,
       string Email,
       string? IdentificationNumber,
       DateOnly? BirthDate,
       string PhoneNumber,
       CustomerStatus Status);


    public sealed class Validator : AbstractValidator<UpdateCustomerRequest>
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

            RuleFor(r => r.Status)
                .IsInEnum();
        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("customers/{customerId}", Handler)
                .RequireAuthorization()
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithTags(Tags.Customers);
        }
    }

    public static async Task<IResult> Handler(
        string customerId,
        UpdateCustomerRequest request,
        ApplicationDbContext context,
        IValidator<UpdateCustomerRequest> validator)
    {
        Customer? customer = await context.Customers.FindAsync(customerId);

        if(customer is null)
        {
            return Results.NotFound(customerId);
        }

        await validator.ValidateAndThrowAsync(request);

        customer = request.ToEntity(customer);

        context.Customers.Update(customer);

        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
