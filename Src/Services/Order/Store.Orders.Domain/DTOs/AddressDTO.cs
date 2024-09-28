using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Orders.Domain.DTOs
{
    public class AddressDTO
    {
        public string StreetAddress { get; set; }
        public string BuildingNumber { get; set; }
        public string SecondaryAddress { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
