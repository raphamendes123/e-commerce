using Store.Orders.Domain.DTOs;
using Store.Orders.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Orders.Domain.Data.Entitys.Orders;

namespace Store.Orders.Domain.Extensions
{
    public static class OrderItemsExtensions
    {
        public static OrderItemEntity ToOrderItemEntity(this OrderItemDTO orderItemDto)
        {
            return new OrderItemEntity(
                idProduct: orderItemDto.IdProduct,
                productName: orderItemDto.Name,
                quantity: orderItemDto.Quantity,
                price: orderItemDto.Price,
                productImage: orderItemDto.Image);
        }
    }
}
