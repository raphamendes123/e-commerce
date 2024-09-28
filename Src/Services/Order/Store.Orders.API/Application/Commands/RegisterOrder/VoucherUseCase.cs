using Core.SpecificationsUseCase;
using Store.Orders.Domain.Data.Entitys.Vouchers;
using Store.Orders.Domain.Specifications;

namespace Store.Orders.API.Application.Commands.RegisterOrder
{
    public class VoucherUseCase : Validator<VoucherEntity>
    {        
        public VoucherUseCase() {
            Add("VoucherActiveSpecification", new Rule<VoucherEntity>(new VoucherActiveSpecification(), "voucher inactive"));
            Add("VoucherExpirationDateSpecification", new Rule<VoucherEntity>(new VoucherExpirationDateSpecification(), "voucher expired"));
            Add("VoucherQuantitySpecification", new Rule<VoucherEntity>(new VoucherQuantitySpecification(), "voucher no quantity  "));
        }
    }
}
