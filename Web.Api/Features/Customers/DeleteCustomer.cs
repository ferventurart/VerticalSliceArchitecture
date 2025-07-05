using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;

namespace Web.Api.Features.Customers;

public static class DeleteCustomer
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("customers/{customerId}", Handler)
                .RequireAuthorization()
                .WithTags(Tags.Customers);
        }
    }

    public static async Task<IResult> Handler(
        string customerId,
        ApplicationDbContext context)
    {
        Customer? customer = await context.Customers.FindAsync(customerId);

        if (customer is null)
        {
            return Results.NotFound(customerId);
        }

        context.Customers.Remove(customer);

        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
