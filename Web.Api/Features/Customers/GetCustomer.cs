using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.Customers.Common;

namespace Web.Api.Features.Customers;

public static class GetCustomer
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("customers/{customerId}", Handler)
               .WithTags("Customers")
               .Produces<CustomerDto>()
               .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    public static async Task<IResult> Handler(
        string customerId,
        ApplicationDbContext context)
    {
        Customer? customer = await context.Customers.FindAsync(customerId);

        if (customer is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(customer.ToDto());
    }
}

