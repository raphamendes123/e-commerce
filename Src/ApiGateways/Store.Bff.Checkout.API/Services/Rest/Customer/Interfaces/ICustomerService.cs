using Store.Bff.Checkout.API.Models;

namespace Store.Bff.Checkout.API.Services.Rest.Customer.Interfaces
{
    public interface ICustomerService
    {
        Task<AddressDTO> GetAddress();
    }
}
