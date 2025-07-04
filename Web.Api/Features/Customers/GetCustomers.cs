using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common.Dtos;
using Web.Api.Database;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Features.Customers.Common;

namespace Web.Api.Features.Customers;

public static class GetCustomers
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("customers", Handler)
                .RequireAuthorization()
                .WithTags(Tags.Customers);
        }
    }

    public static async Task<IResult> Handler(
        PaginationRequest request,
        ApplicationDbContext context,
        IValidator<PaginationRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);

        IQueryable<Customer> query = context.Customers
            .OrderBy(o => o.FirstName)
            .AsQueryable();

        PaginationResult<Customer> paginationResult = await PaginationResult<Customer>.CreateAsync(
            query, 
            request.Page, 
            request.PageSize);

        var paginationResponse = new PaginationResult<CustomerDto>()
        {
            Items = paginationResult.Items.Select(s => s.ToDto()).ToList(),
            Page = paginationResult.Page,
            PageSize = paginationResult.PageSize,
            TotalCount = paginationResult.TotalCount
        };

        return Results.Ok(paginationResponse);
    }
}
