using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.Products.Common;

namespace Web.Api.Features.Products;

public static class UpdateProduct
{
    public record UpdateProductRequest(
       string ProductCategoryId,
       string Name,
       string? Description,
       decimal Price,
       decimal Cost,
       IFormFile? Image,
       ProductStatus Status);


    public sealed class Validator : AbstractValidator<UpdateProductRequest>
    {
        public Validator(ApplicationDbContext context)
        {
            RuleFor(r => r.ProductCategoryId)
                .NotEmpty()
                .MaximumLength(500)
                 .MustAsync(async (id, cancellation) =>
                    await context.ProductCategories
                                 .AsNoTracking()
                                 .AnyAsync(pc => pc.Id == id, cancellation)
                ).WithMessage("There is no category with the Id '{PropertyValue}'.");

            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(r => r.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(r => r.Price)
                .NotEmpty()
                .GreaterThan(g => g.Cost);

            RuleFor(r => r.Cost)
                .NotEmpty()
                .LessThan(l => l.Price);

            RuleFor(r => r.Status)
                .IsInEnum();
        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("products/{productId}", Handler)
                .RequireAuthorization()
                .DisableAntiforgery()
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithTags(Tags.Products);
        }
    }

    public static async Task<IResult> Handler(
        string productId,
        [FromForm] UpdateProductRequest request,
        ApplicationDbContext context,
        IValidator<UpdateProductRequest> validator)
    {
        Product? product = await context.Products.FindAsync(productId);

        if (product is null)
        {
            return Results.NotFound(productId);
        }

        await validator.ValidateAndThrowAsync(request);

        product = request.ToEntity(product);

        context.Products.Update(product);

        await context.SaveChangesAsync();

        return Results.NoContent();
    }
}
