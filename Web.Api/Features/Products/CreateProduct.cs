using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.Products.Common;

namespace Web.Api.Features.Products;

public static class CreateProduct
{
    public record CreateProductRequest(
        string ProductCategoryId,
        string Name,
        string? Description,
        decimal Price,
        decimal Cost,
        IFormFile? Image);


    public sealed class Validator : AbstractValidator<CreateProductRequest>
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

        }
    }

    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("products", Handler)
                .RequireAuthorization()
                .DisableAntiforgery()
                .WithTags(Tags.Products);
        }
    }

    public static async Task<IResult> Handler(
        [FromForm] CreateProductRequest request,
        ApplicationDbContext context,
        IValidator<CreateProductRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);

        var product = new Product
        {
            Id = Product.NewId(),
            ProductCategoryId = request.ProductCategoryId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Cost = request.Cost,
            ImageUri = null,
            Status = ProductStatus.Active
        };

        context.Products.Add(product);

        await context.SaveChangesAsync();

        return Results.Created($"/products/{product.Id}", product.ToDto());
    }
}
