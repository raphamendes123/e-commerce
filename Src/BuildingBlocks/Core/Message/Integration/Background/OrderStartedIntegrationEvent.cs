using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Message.Integration.Background
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid IdCustomer { get; set; }
        public Guid IdOrder { get; set; }

        public int TypePayment { get; set; }
        public decimal Amount { get; set; }

        public string Holder { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
    }
}
