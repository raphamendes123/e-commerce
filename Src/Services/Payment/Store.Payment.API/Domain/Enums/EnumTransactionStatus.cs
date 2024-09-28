namespace Store.Payment.API.Domain.Enums
{
    public enum EnumTransactionStatus
    {
        Authorized = 1, //AUTORIZADO
        Paid, //PAGO
        Denied, //NEGADO
        Refund, //REEMBOLSO
        Canceled //CANCELADO
    }
}
