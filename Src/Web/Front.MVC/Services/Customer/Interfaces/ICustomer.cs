using Core.Domain.ResponseResult;
using Front.MVC.Models;

namespace Front.MVC.Services.Customer.Interfaces
{
    public interface ICustomerService
    {
        Task<AddressViewModel> GetAddress();
        Task<ResponseResult> AddAddress(AddressViewModel address);
        Task<ResponseResult> UpdateAddress(AddressViewModel address);
    }
}
