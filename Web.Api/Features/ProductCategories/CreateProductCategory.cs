using FluentValidation;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.ProductCategories.Common;

namespace Web.Api.Features.ProductCategories;

public static class CreateProductCategory
{
    public record CreateProductCategoryRequest(string Name);


    public sealed class Validator : AbstractValidator<CreateProductCategoryRequest>
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
            app.MapPost("product-categories", Handler)
                .RequireAuthorization()
                .WithTags(Tags.ProductCategories);
        }
    }

    public static async Task<IResult> Handler(
        CreateProductCategoryRequest request,
        ApplicationDbContext context,
        IValidator<CreateProductCategoryRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);

        var productCategory = new ProductCategory
        {
            Id = ProductCategory.NewId(),
            Name = request.Name
        };

        context.ProductCategories.Add(productCategory);

        await context.SaveChangesAsync();

        return Results.Created($"/product-categories/{productCategory.Id}", productCategory.ToDto());
    }
}
