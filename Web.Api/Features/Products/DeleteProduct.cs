using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;

namespace Web.Api.Features.Products;

public static class DeleteProduct
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("products/{productId}", Handler)
                .RequireAuthorization()
                .WithTags(Tags.Products);
        }
    }

    public static async Task<IResult> Handler(
        string productId,
        ApplicationDbContext context)
    {
        Product? product = await context.Products.FindAsync(productId);

        if (product is null)
        {
            return Results.NotFound(productId);
        }

        context.Products.Remove(product);

        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
