using FluentValidation;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.ProductCategories.Common;

namespace Web.Api.Features.ProductCategories;

public static class UpdateProductCategory
{
    public record UpdateProductCategoryRequest(string Name);


    public sealed class Validator : AbstractValidator<UpdateProductCategoryRequest>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("product-categories/{productCategoryId}", Handler)
                .RequireAuthorization()
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithTags(Tags.ProductCategories);
        }
    }

    public static async Task<IResult> Handler(
        string productCategoryId,
        UpdateProductCategoryRequest request,
        ApplicationDbContext context,
        IValidator<UpdateProductCategoryRequest> validator)
    {
        ProductCategory? productCategory = await context.ProductCategories.FindAsync(productCategoryId);

        if (productCategory is null)
        {
            return Results.NotFound(productCategoryId);
        }

        await validator.ValidateAndThrowAsync(request);

        productCategory = request.ToEntity(productCategory);

        context.ProductCategories.Update(productCategory);

        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
