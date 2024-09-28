using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Payment.Pay.Enums
{
    public enum EnumTransactionStatus
    {
        Authorized = 1,
        Paid,
        Refused,
        Chargeback,
        Cancelled
    }
}
