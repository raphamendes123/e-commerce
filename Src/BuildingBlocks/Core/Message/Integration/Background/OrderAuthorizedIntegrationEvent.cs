using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Message.Integration.Background
{
    public class OrderAuthorizedIntegrationEvent : IntegrationEvent
    {
        public Guid IdCustomer { get; private set; }
        public Guid IdOrder { get; private set; }
        public IDictionary<Guid, int> Items { get; private set; }

        public OrderAuthorizedIntegrationEvent(Guid idCustomer, Guid idOrder, IDictionary<Guid, int> items = null)
        {
            IdCustomer = idCustomer;
            IdOrder = idOrder;
            Items = items;
        }
    }
}
