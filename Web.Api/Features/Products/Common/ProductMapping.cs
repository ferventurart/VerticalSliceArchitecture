using Web.Api.Entities;
using static Web.Api.Features.Products.UpdateProduct;

namespace Web.Api.Features.Products.Common;

internal static class ProductMapping
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.ProductCategoryId,
            product.Name,
            product.Description,
            product.Price,
            product.Cost,
            product.Profit,
            product.ImageUri,
            product.Status.ToString());
    }

    public static Product ToEntity(this UpdateProductRequest dto, Product product)
    {
        product.ProductCategoryId = dto.ProductCategoryId;
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Cost = dto.Cost;
        product.Status = dto.Status;

        return product;
    }
}
