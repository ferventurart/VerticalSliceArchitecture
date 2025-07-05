using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.ProductCategories.Common;

namespace Web.Api.Features.ProductCategories;

public static class GetProductCategory
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("product-categories/{productCategoryId}", Handler)
               .RequireAuthorization()
               .WithTags(Tags.ProductCategories)
               .Produces<ProductCategoryDto>()
               .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    public static async Task<IResult> Handler(
        string productCategoryId,
        ApplicationDbContext context)
    {
        ProductCategory? productCategory = await context.ProductCategories.FindAsync(productCategoryId);

        if (productCategory is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(productCategory.ToDto());
    }
}

