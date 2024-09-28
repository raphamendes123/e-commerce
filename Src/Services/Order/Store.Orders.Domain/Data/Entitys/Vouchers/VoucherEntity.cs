using Core.Domain.Repository.Data;
using Core.Domain.Repository.DomainObjects;
using Store.Orders.Domain.Enums;
using Store.Orders.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Orders.Domain.Data.Entitys.Vouchers
{
    public class VoucherEntity : Entity, IAggregateRoot
    {
        public string Code { get; private set; }
        public decimal? Percentage { get; private set; }
        public decimal? Discount { get; private set; }
        public int Quantity { get; private set; }
        public VoucherDiscountType DiscountType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UsedAt { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        public VoucherEntity(string code, decimal? percentage, decimal? discount, int quantity, VoucherDiscountType discountType, DateTime expirationDate)
        {
            Code = code;
            Percentage = percentage;
            Discount = discount;
            Quantity = quantity;
            DiscountType = discountType;
            ExpirationDate = expirationDate;

            CreatedAt = DateTime.Now;
            Active = true;
            Used = false;
        }

        public bool IsValid()
        {
            return new VoucherActiveSpecification()
                .And(new VoucherExpirationDateSpecification())
                .And(new VoucherQuantitySpecification())
                .IsSatisfiedBy(this);
        }

        public void SetAsUsed()
        {
            Active = false;
            Used = true;
            Quantity = 0;
            UsedAt = DateTime.Now;
        }

        public void SubtractVoucher()
        {
            Quantity -= 1;
            if (Quantity >= 1) return;

            SetAsUsed();
        }
    }
}
