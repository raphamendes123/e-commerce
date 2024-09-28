using Store.Orders.Domain.Data.Entitys.Vouchers;
using Store.Orders.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Orders.Domain.Extensions
{
    public static class VoucherExtensions
    {
        public static VoucherDTO toDTO(this VoucherEntity voucherEntity)
        {
            return new VoucherDTO()
            {
                Code = voucherEntity.Code,
                Percentage = voucherEntity.Percentage,
                Discount = voucherEntity.Discount,
                DiscountType = voucherEntity.DiscountType 
            };
        }
    }
}
