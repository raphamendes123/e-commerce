using Core.Domain.Repository.Data;
using Core.Domain.Repository.DomainObjects;
using Store.Orders.Domain.Data.Entitys.Vouchers;
using Store.Orders.Domain.Enums;
using Store.Orders.Infra.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Store.Orders.Domain.Data.Entitys.Orders
{
    public class OrderEntity : Entity, IAggregateRoot
    {
        public OrderEntity(Guid idCustomer, decimal amount, List<OrderItemEntity> orderItems, bool hasVoucher = false, decimal discount = 0, Guid? idVoucher = null)
        {
            IdCustomer = idCustomer;
            Amount = amount;
            _orderItems = orderItems;

            Discount = discount;
            HasVoucher = hasVoucher;
            IdVoucher = idVoucher;
        }

        // EF ctor
        protected OrderEntity() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; private set; }
        public Guid IdCustomer { get; private set; }
        public Guid? IdVoucher { get; private set; }
        public bool HasVoucher { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime DateAdded { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItemEntity> _orderItems;
        public IReadOnlyCollection<OrderItemEntity> OrderItems => _orderItems;

        public Address Address { get; private set; }

        // EF Rel.
        public VoucherEntity? Voucher { get; private set; }

        public void Authorized()
        {
            OrderStatus = OrderStatus.Authorized;
        }

        public void Cancel()
        {
            OrderStatus = OrderStatus.Canceled;
        }

        public void Finish()
        {
            OrderStatus = OrderStatus.Paid;
        }

        public void SetVoucher(VoucherEntity voucher)
        {
            HasVoucher = true;
            IdVoucher = voucher.Id;
            Voucher = voucher;
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }

        public void CalculateOrderAmount()
        {
            Amount = OrderItems.Sum(p => p.CalculateAmount());
            CalculateAmount();
        }

        public void CalculateAmount()
        {
            if (!HasVoucher) return;

            decimal discount = 0;
            var amount = Amount;

            if (Voucher.DiscountType == VoucherDiscountType.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    discount = amount * Voucher.Percentage.Value / 100;
                    amount -= discount;
                }
            }
            else
            {
                if (Voucher.Discount.HasValue)
                {
                    discount = Voucher.Discount.Value;
                    amount -= discount;
                }
            }

            Amount = amount < 0 ? 0 : amount;
            Discount = discount;
        }
    }
}