using System.Reflection;
using FluentValidation;

namespace Web.Api.Common.Dtos;

public record PaginationRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public static ValueTask<PaginationRequest> BindAsync(HttpContext context, ParameterInfo _)
    {
        IQueryCollection q = context.Request.Query;
        int page = int.TryParse(q["page"], out int p) ? p : 1;
        int pageSize = int.TryParse(q["pageSize"], out int ps) ? ps : 10;
        return ValueTask.FromResult(new PaginationRequest { Page = page, PageSize = pageSize });
    }
}

public sealed class Validator : AbstractValidator<PaginationRequest>
{
    public Validator()
    {
        RuleFor(r => r.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.PageSize)
            .GreaterThanOrEqualTo(10);
    }
}
