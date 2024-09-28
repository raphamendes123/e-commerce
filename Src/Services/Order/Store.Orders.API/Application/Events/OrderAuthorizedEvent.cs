using Core.Message;

namespace Store.Orders.API.Application.Events
{
    public class OrderAuthorizedEvent : Event
    {
        public Guid IdOrder { get; private set; }
        public Guid IdCustomer { get; private set; }

        public OrderAuthorizedEvent(Guid idOrder, Guid idCustomer)
        {
            IdOrder = idOrder;
            IdCustomer = idCustomer;
        }
    }
}
