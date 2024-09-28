using Core.SpecificationsFunc;
using Store.Orders.Domain.Data.Entitys.Vouchers;
using System.Linq.Expressions;

namespace Store.Orders.Domain.Specifications
{
    public class VoucherExpirationDateSpecification : Specification<VoucherEntity>
    {
        public override Expression<Func<VoucherEntity, bool>> ToExpression()
        {
            return voucher => voucher.ExpirationDate >= DateTime.Now;
        }
    }
}
