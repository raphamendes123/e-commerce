using Core.Domain.Repository.DomainObjects;
using Core.Message;
using FluentValidation;
using Store.Customer.API.Application.Commands.RegisterAddress;

namespace Store.Customer.API.Application.Commands
{
    public class RegisterAddressCommand : Command
    {
        public Guid IdCustomer { get; set; }
        public string StreetAddress { get; set; }
        public string BuildingNumber { get; set; }
        public string SecondaryAddress { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public RegisterAddressCommand()
        {
        }

        public RegisterAddressCommand(
            Guid idCustomer, 
            string streetAddress, 
            string buildingNumber, 
            string secondaryAddress,
            string neighborhood, 
            string zipCode, 
            string city, 
            string state)
        {
            AggregateId = idCustomer;
            IdCustomer = idCustomer;
            StreetAddress = streetAddress;
            BuildingNumber = buildingNumber;
            SecondaryAddress = secondaryAddress;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterAddressValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        
    }
}
