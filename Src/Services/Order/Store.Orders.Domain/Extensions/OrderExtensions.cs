using Store.Orders.Domain.Data.Entitys.Orders;
using Store.Orders.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Orders.Domain.Extensions
{
    public static class OrderExtensions
    {
        public static OrderDTO ToOrderDTO(this OrderEntity order)
        {
            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                IdCustomer = order.IdCustomer,
                Code = order.Code,
                Status = (int)order.OrderStatus,
                Date = order.DateAdded,
                Amount = order.Amount,
                Discount = order.Discount,
                HasVoucher = order.HasVoucher,
                OrderItems = new List<OrderItemDTO>(),
                Address = new AddressDTO()
            };

            foreach (var item in order.OrderItems)
            {
                orderDTO.OrderItems.Add(new OrderItemDTO
                {
                    Name = item.ProductName,
                    Image = item.ProductImage,
                    Quantity = item.Quantity,
                    IdProduct = item.IdProduct,
                    Price = item.Price,
                    IdOrder = item.IdOrder
                });
            }

            orderDTO.Address = new AddressDTO
            {
                StreetAddress = order.Address.StreetAddress,
                BuildingNumber = order.Address.BuildingNumber,
                SecondaryAddress = order.Address.SecondaryAddress,
                Neighborhood = order.Address.Neighborhood,
                ZipCode = order.Address.ZipCode,
                City = order.Address.City,
                State = order.Address.State,
            };

            return orderDTO;
        }
    }
}
