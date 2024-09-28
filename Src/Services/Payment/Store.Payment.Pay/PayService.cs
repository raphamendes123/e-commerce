using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Payment.Pay
{
    public class PayService
    {
        public readonly string ApiKey;
        public readonly string EncryptionKey;

        public PayService(string apiKey, string encryptionKey)
        {
            ApiKey = apiKey;
            EncryptionKey = encryptionKey;
        }
    }
}
