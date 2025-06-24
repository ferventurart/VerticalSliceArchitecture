using Web.Api.Entities;
using Web.Api.Features.Common;

namespace Web.Api.Features.Customers;
internal static class CustomerMapping
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.IdentificationNumber,
            customer.BirthDate,
            customer.PhoneNumber,
            customer.Status.ToString());
    }
}
