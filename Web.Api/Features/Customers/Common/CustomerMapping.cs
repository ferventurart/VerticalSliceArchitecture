using Web.Api.Entities;
using static Web.Api.Features.Customers.UpdateCustomer;

namespace Web.Api.Features.Customers.Common;
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

    public static Customer ToEntity(this UpdateCustomerRequest dto, Customer customer)
    {
        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Email = dto.Email;
        customer.IdentificationNumber = dto.IdentificationNumber;
        customer.BirthDate = dto.BirthDate;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.Status = dto.Status;

        return customer;
    }
}
