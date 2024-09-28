using Store.Customer.API.Application.Commands;
using Store.Customer.API.Domain.Data.Entitys;

namespace Store.Customer.API.Application.Extensions
{
    public static class UpdateAddressCommandExtensions
    {
        public static AddressEntity ToAddressEntity(this UpdateAddressCommand command)
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
                command.Id);
        }
    }
}
