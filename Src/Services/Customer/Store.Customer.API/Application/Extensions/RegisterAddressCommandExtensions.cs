using Store.Customer.API.Application.Commands;
using Store.Customer.API.Domain.Data.Entitys;

namespace Store.Customer.API.Application.Extensions
{
    public static class RegisterAddressCommandExtensions
    {
        public static AddressEntity ToAddressEntity(this RegisterAddressCommand command)
        {

            return new AddressEntity(
                command.StreetAddress,
                command.BuildingNumber,
                command.SecondaryAddress,
                command.Neighborhood,
                command.ZipCode,
                command.City,
                command.State,
                command.IdCustomer,
                Guid.NewGuid());
        }
    }
}
