using Core.Domain.ResponseResult; 
using Store.Bff.Checkout.Extensions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Store.Bff.Checkout.Services
{
    public abstract class Service
    {
        protected StringContent GetContent(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        public bool ResponseErrors(HttpResponseMessage response)
        {
            if(response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        public ResponseResult ReturnOK()
        {
            return new ResponseResult();
        }

    }
}
