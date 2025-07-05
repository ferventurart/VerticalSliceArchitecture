using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.Products.Common;

namespace Web.Api.Features.Products;

public static class GetProduct
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products/{productId}", Handler)
               .RequireAuthorization()
               .WithTags(Tags.Products)
               .Produces<ProductDto>()
               .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    public static async Task<IResult> Handler(
        string productId,
        ApplicationDbContext context)
    {
        Product? product = await context.Products.FindAsync(productId);

        if (product is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(product.ToDto());
    }
}
