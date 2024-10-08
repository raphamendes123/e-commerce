namespace Store.Orders.Infra.Data.Models
{
    public class Address
    {
        public string? StreetAddress { get; set; }
        public string? BuildingNumber { get; set; }
        public string? SecondaryAddress { get; set; }
        public string? Neighborhood { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}