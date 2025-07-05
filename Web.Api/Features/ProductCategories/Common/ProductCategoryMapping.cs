using Web.Api.Entities;
using static Web.Api.Features.ProductCategories.UpdateProductCategory;

namespace Web.Api.Features.ProductCategories.Common;

internal static class ProductCategoryMapping
{
    public static ProductCategoryDto ToDto(this ProductCategory productCategory)
    {
        return new ProductCategoryDto(
            productCategory.Id,
            productCategory.Name);
    }

    public static ProductCategory ToEntity(this UpdateProductCategoryRequest dto, ProductCategory productCategory)
    {
        productCategory.Name = dto.Name;

        return productCategory;
    }
}
