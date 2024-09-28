using Core.Message.Integration;
using Store.Payment.API.Domain.Data.Entitys;

namespace Store.Payment.API.Services.Interfaces
{

    //RETORNO PARA COMPRAS BFF >> MVC
    public interface IPaymentService
    {       
        Task<ResponseMessage> AuthorizePayment(PaymentEntity payment);
        Task<ResponseMessage> GetTransaction(Guid idOrder);
        Task<ResponseMessage> CancelTransaction(Guid idOrder);
    }
}
