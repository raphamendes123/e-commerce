using Core.Domain.ResponseResult;
using Front.MVC.Extensions;
using System.Text;
using System.Text.Json;

namespace Front.MVC.Services.Abstracts
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

        public bool HandleResponseErrors(HttpResponseMessage Response)
        {
            switch ((int)Response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(Response.StatusCode);

                case 400:
                    return false;
            }

            Response.EnsureSuccessStatusCode();
            return true;
        }

        public ResponseResult ReturnOK()
        {
            return new ResponseResult();
        }

    }
}
