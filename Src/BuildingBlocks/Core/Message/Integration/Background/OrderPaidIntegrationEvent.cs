using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Message.Integration.Background
{
    public class OrderPaidIntegrationEvent : IntegrationEvent
    {
        public Guid IdCustomer { get; private set; }
        public Guid IdOrder { get; private set; }

        public OrderPaidIntegrationEvent(Guid idCustomer, Guid idOrder)
        {
            IdCustomer = idCustomer;
            IdOrder = idOrder;
        }
    }
}
