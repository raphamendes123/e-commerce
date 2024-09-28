using Store.Orders.API.Application.Commands.RegisterOrder;
using Store.Orders.Domain.Data.Entitys.Orders;
using Store.Orders.Domain.DTOs;
using Store.Orders.Domain.Extensions;
using Store.Orders.Infra.Data.Models;

namespace Store.Orders.API.Application.Extensions
{
    public static class RegisterOrderCommandExtensions
    {
        public static OrderEntity toOrderEntity(this RegisterOrderCommand command)
        {   
            var address = new Address
            {
                StreetAddress = command.Address.StreetAddress,
                BuildingNumber = command.Address.BuildingNumber,
                SecondaryAddress = command.Address.SecondaryAddress,
                Neighborhood = command.Address.Neighborhood,
                ZipCode = command.Address.ZipCode,
                City = command.Address.City,
                State = command.Address.State
            };

            var order = 
                new OrderEntity(
                command.IdCustomer, 
                command.Amount,
                command.OrderItems.Select(x => x.ToOrderItemEntity()).ToList(),
                command.HasVoucher, 
                command.Discount);

            order.SetAddress(address);
            return order;
        }
    }
}
