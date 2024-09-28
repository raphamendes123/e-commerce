using System.Text.Json;
using System.Text;

namespace Store.Bff.Checkout.Extensions
{
    public static class StringContentExtensions
    {
        public static StringContent ToJson(this object value)
        {
            StringContent? stringContent = new StringContent
            (
                content: JsonSerializer.Serialize(value),
                encoding: Encoding.UTF8,
                mediaType: "Application/json"
            );

            return stringContent;
        }

        public async static Task<T> ToJsonAsync<T>(this Task<string> value)
        {
            JsonSerializerOptions? options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(await value, options);
        }

        public async static Task<T> DeserializerResponse<T>(this HttpResponseMessage httpResponseMessage)
        {
            JsonSerializerOptions? options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(await httpResponseMessage.Content.ReadAsStringAsync(), options);
        }
    }
}
