using Microsoft.AspNetCore.Mvc.Formatters;
using Store.Orders.API.Application.Queries.Interface;
using Store.Orders.Domain.Data.Entitys.Vouchers;
using Store.Orders.Domain.DTOs;
using Store.Orders.Domain.Extensions;
using Store.Orders.Infra.Data.Repositorys.Interfaces;

namespace Store.Orders.API.Application.Queries
{
    public class VoucherQuerie : IVoucherQuerie
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherQuerie(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }


        //validar o voucher

        public async Task<VoucherDTO> GetCodeAsync(string code)
        {
            VoucherEntity? voucher = await _voucherRepository.GetCodeAsync(code);

            if (voucher is null || !voucher.IsValid())
            {                
                return null;
            }
            
            return voucher.toDTO();
        }
    }
}
