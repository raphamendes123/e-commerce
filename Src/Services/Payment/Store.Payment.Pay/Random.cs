using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Payment.Pay
{
    public static class RandomString
    {
        private static readonly Random random = new Random();
        public static string Random(int length = 5)
        {
            const string chars = "515426121dsaSAdsaDSDSADSADSA";
            var content = new char[length];
            for (int i = 0; i < length; i++)
            {
                content[i] = chars[random.Next(chars.Length)];
            }

            return new string(content);
        }
    }
}
