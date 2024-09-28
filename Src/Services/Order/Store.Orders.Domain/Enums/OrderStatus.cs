namespace Store.Orders.Domain.Enums
{
    public enum OrderStatus : int
    {
        Authorized = 1,
        Paid = 2,
        Refused = 3,
        Delivered = 4,
        Canceled = 5
    }
}