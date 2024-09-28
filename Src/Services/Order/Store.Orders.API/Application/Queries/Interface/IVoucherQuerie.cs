using Store.Orders.Domain.DTOs;

namespace Store.Orders.API.Application.Queries.Interface
{
    public interface IVoucherQuerie 
    {
        Task<VoucherDTO> GetCodeAsync(string code);
    }
}
