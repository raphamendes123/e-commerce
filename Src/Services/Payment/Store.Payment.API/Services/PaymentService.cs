using Core.Domain.Repository.DomainObjects;
using Core.Message.Integration;
using FluentValidation.Results;
using Store.Payment.API.Domain.Data.Entitys;
using Store.Payment.API.Domain.Data.Repository.Interfaces;
using Store.Payment.API.Domain.Enums;
using Store.Payment.API.Facade.Interfaces;
using Store.Payment.API.Services.Interfaces;

namespace Store.Payment.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentFacade _paymentFacade;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentFacade paymentFacade, IPaymentRepository paymentRepository)
        {
            _paymentFacade = paymentFacade;
            _paymentRepository = paymentRepository;
        }

        public async Task<ResponseMessage> AuthorizePayment(PaymentEntity payment)
        {
            var transaction = await _paymentFacade.AuthorizePayment(payment);

            var validationResult = new ValidationResult();

            if (transaction.TransactionStatus != EnumTransactionStatus.Authorized)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                        "Payment refused, please contact your card operator"));

                return new ResponseMessage(validationResult);
            }

            payment.AddTransaction(transaction);
            _paymentRepository.AddPayment(payment);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    "There was an error while making the payment."));

                //TODO: Comunicar para o gateway para realizar o estorno do pagamento PAGO
                //Via bus

                // Canceling the payment on the service
                //await CancelTransaction(payment.OrderId);

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CancelTransaction(Guid idOrder)
        {
            var transactions = await _paymentRepository.GetTransactionsByIdOrder(idOrder);
            var authorizedTransaction = transactions?.FirstOrDefault(t => t.TransactionStatus == EnumTransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if (authorizedTransaction is null) throw new DomainException($"Transaction not found for order {idOrder}");

            var transaction = await _paymentFacade.CancelAuthorization(authorizedTransaction);

            if (transaction.TransactionStatus != EnumTransactionStatus.Canceled)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    $"Unable to cancel order payment {idOrder}"));

                return new ResponseMessage(validationResult);
            }

            transaction.IdPayment = authorizedTransaction.IdPayment;
            _paymentRepository.AddTransaction(transaction);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    $"It was not possible to persist the cancellation of the order payment {idOrder}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
     

        public async Task<ResponseMessage> GetTransaction(Guid idOrder)
        {
            var transactions = await _paymentRepository.GetTransactionsByIdOrder(idOrder);
            var authorizedTransaction = transactions?.FirstOrDefault(t => t.TransactionStatus == EnumTransactionStatus.Authorized);
            var validationResult = new ValidationResult();

            if (authorizedTransaction is null) 
                throw new DomainException($"Transaction not found for order {idOrder}");

            var transaction = await _paymentFacade.CapturePayment(authorizedTransaction);

            if (transaction.TransactionStatus != EnumTransactionStatus.Paid)
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    $"Unable to capture order payment {idOrder}"));

                return new ResponseMessage(validationResult);
            }

            transaction.IdPayment = authorizedTransaction.IdPayment;
            _paymentRepository.AddTransaction(transaction);

            if (!await _paymentRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Payment",
                    $"It was not possible to persist the capture of the payment of the order {idOrder}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}
