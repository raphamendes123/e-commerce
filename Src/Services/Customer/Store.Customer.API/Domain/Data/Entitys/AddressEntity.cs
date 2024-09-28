using Core.Domain.Repository.DomainObjects;

namespace Store.Customer.API.Domain.Data.Entitys
{
    public class AddressEntity : Entity
    {
        public string StreetAddress { get; private set; }
        public string BuildingNumber { get; private set; }
        public string SecondaryAddress { get; private set; }
        public string Neighborhood { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public Guid CustomerId { get; private set; }

        // EF Relation
        public CustomerEntity Customer { get; protected set; }        

        // EF Constructor
        protected AddressEntity() { }

        public AddressEntity(string streetAddress, string buildingNumber, string secondaryAddress, string neighborhood, string zipCode, string city, string state, Guid customerId, Guid id)
        {
            StreetAddress = streetAddress;
            BuildingNumber = buildingNumber;
            SecondaryAddress = secondaryAddress;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            CustomerId = customerId;
            Id = id;
        }
    }
}
