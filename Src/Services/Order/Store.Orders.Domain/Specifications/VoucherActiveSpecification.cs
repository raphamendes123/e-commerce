using Core.SpecificationsFunc;
using Store.Orders.Domain.Data.Entitys.Vouchers;
using System.Linq.Expressions;

namespace Store.Orders.Domain.Specifications
{
    public class VoucherActiveSpecification : Specification<VoucherEntity>
    {
        public override Expression<Func<VoucherEntity, bool>> ToExpression()
        {
            return voucher => voucher.Active && !voucher.Used;
        }
    }
}
