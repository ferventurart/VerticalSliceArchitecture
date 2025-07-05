using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;

namespace Web.Api.Features.ProductCategories;

public static class DeleteProductCategory
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("product-categories/{productCategoryId}", Handler)
                .RequireAuthorization()
                .WithTags(Tags.ProductCategories);
        }
    }

    public static async Task<IResult> Handler(
        string productCategoryId,
        ApplicationDbContext context)
    {
        ProductCategory? productCategory = await context.ProductCategories.FindAsync(productCategoryId);

        if (productCategory is null)
        {
            return Results.NotFound(productCategoryId);
        }

        context.ProductCategories.Remove(productCategory);

        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
