using Core.Message;
using Core.Message.Integration;
using Core.Message.Integration.Background;
using EasyNetQ;
using FluentValidation.Results;
using MediatR;
using MessageBus;
using Store.Orders.API.Application.Events;
using Store.Orders.API.Application.Extensions;
using Store.Orders.Domain.Data.Entitys.Orders;
using Store.Orders.Infra.Data.Repositorys.Interfaces;

namespace Store.Orders.API.Application.Commands.RegisterOrder
{
    public class RegisterOrderHandler : 
        CommandHandler,
        IRequestHandler<RegisterOrderCommand, ValidationResult>
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBus _bus;

        public RegisterOrderHandler(IVoucherRepository voucherRepository, IOrderRepository orderRepository, IMessageBus bus)
        {
            _voucherRepository = voucherRepository;
            _orderRepository = orderRepository;
            _bus = bus;
        }

        public async Task<ValidationResult> Handle(RegisterOrderCommand request, CancellationToken cancellationToken)
        {
            //validacao do request
            if (!request.IsValid()) return request.ValidationResult;

            // mapear pedido
            OrderEntity? order = request.toOrderEntity();

            // aplicar voucher 
            if (!await ApplyVoucher(request, order)) return ValidationResult;

            // validacao do pedido x cart
            if (!IsOrderValid(order)) return ValidationResult;

            // Enviar o pedido de pagamento event e aguardar a resposta.
            if (!await SendPaymentEvent(request, order)) 
                return ValidationResult;

            // se pagamento tudo ok
            order.Authorized();

            // adicionar evento
            order.AddEvent(new OrderAuthorizedEvent(order.Id, order.IdCustomer));

            // adicionar pedido repositorio
            _orderRepository.Add(order);

            // persistir dados de pedido e voucher
            return await PersistData(_orderRepository.UnitOfWork);
        }

        private async Task<bool> SendPaymentEvent(RegisterOrderCommand message, OrderEntity order)
        {
            OrderStartedIntegrationEvent? orderStarted = new OrderStartedIntegrationEvent()
            {
                IdCustomer = message.IdCustomer,
                IdOrder= order.Id,
                TypePayment = 1,//DEIXAR DINAMICO
                Amount = order.Amount,
                Holder = message.Holder,
                CardNumber  = message.CardNumber,
                ExpirationDate = message.ExpirationDate,
                SecurityCode = message.SecurityCode,
            };

            var result = await _bus.RequestAsync<OrderStartedIntegrationEvent, ResponseMessage>(orderStarted);

            if (!result.ValidationResult.IsValid)
            {
                foreach (var error in result?.ValidationResult?.Errors)
                {
                    AddError(error.ErrorMessage);
                }
                return false;
            }

            return true;
        }

        private async Task<bool> ApplyVoucher(RegisterOrderCommand message, OrderEntity order)
        {
            if (!message.HasVoucher) return true;

            var voucher = await _voucherRepository.GetCodeAsync(message.Voucher);
            if (voucher == null)
            {
                AddError("Voucher not found!");
                return false;
            }

            var voucherValidation = await new VoucherUseCase().ValidateAsync(voucher);
            if (!voucherValidation.IsValid)
            {
                voucherValidation.Errors.ToList().ForEach(m => AddError(m.Message));
                return false;
            }

            order.SetVoucher(voucher);
            voucher.SubtractVoucher();

            _voucherRepository.Update(voucher);

            return true;
        }

        private bool IsOrderValid(OrderEntity order)
        {
            decimal orderAmount = order.Amount;
            decimal orderDiscount = order.Discount;

            order.CalculateOrderAmount();

            if (order.Amount != orderAmount)
            {
                AddError("The order total amount order is different from total amount of individual items");
                return false;
            }

            if (order.Discount != orderDiscount)
            {
                AddError("The amount sent is different from order amount");
                return false;
            }

            return true;
        }
    }
}
