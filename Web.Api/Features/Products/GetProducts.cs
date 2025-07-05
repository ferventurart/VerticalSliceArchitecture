using FluentValidation;
using Web.Api.Common.Dtos;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.Products.Common;

namespace Web.Api.Features.Products;

public static class GetProducts
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("products", Handler)
                .RequireAuthorization()
                .Produces<PaginationResult<ProductDto>>()
                .WithTags(Tags.Products);
        }
    }

    public static async Task<IResult> Handler(
        PaginationRequest request,
        ApplicationDbContext context,
        IValidator<PaginationRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);

        IQueryable<Product> query = context.Products
            .OrderBy(o => o.Name)
            .AsQueryable();

        PaginationResult<Product> paginationResult = await PaginationResult<Product>.CreateAsync(
            query,
            request.Page,
            request.PageSize);

        var paginationResponse = new PaginationResult<ProductDto>()
        {
            Items = paginationResult.Items.Select(s => s.ToDto()).ToList(),
            Page = paginationResult.Page,
            PageSize = paginationResult.PageSize,
            TotalCount = paginationResult.TotalCount
        };

        return Results.Ok(paginationResponse);
    }
}
