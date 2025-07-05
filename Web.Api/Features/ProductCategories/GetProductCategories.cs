using FluentValidation;
using Web.Api.Common.Dtos;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.ProductCategories.Common;
using Web.Api.Features.Products.Common;

namespace Web.Api.Features.ProductCategories;

public static class GetProductCategories
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("product-categories", Handler)
                .RequireAuthorization()
                .Produces<PaginationResult<ProductCategoryDto>>()
                .WithTags(Tags.ProductCategories);
        }
    }

    public static async Task<IResult> Handler(
        PaginationRequest request,
        ApplicationDbContext context,
        IValidator<PaginationRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);

        IQueryable<ProductCategory> query = context.ProductCategories
            .OrderBy(o => o.Name)
            .AsQueryable();

        PaginationResult<ProductCategory> paginationResult = await PaginationResult<ProductCategory>.CreateAsync(
            query,
            request.Page,
            request.PageSize);

        var paginationResponse = new PaginationResult<ProductCategoryDto>()
        {
            Items = paginationResult.Items.Select(s => s.ToDto()).ToList(),
            Page = paginationResult.Page,
            PageSize = paginationResult.PageSize,
            TotalCount = paginationResult.TotalCount
        };

        return Results.Ok(paginationResponse);
    }
}
