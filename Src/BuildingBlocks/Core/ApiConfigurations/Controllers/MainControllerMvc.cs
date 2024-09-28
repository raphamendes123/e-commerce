using Core.Domain.ResponseResult;
using Microsoft.AspNetCore.Mvc;

namespace Core.ApiConfigurations
{
    public class MainControllerMvc : Controller
    {

        protected bool HasErrors(ResponseResult response)
        {
            if (response != null && response.Errors.Messages.Any())
            {
                foreach (string error in response.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return true;
            }

            return false;
        }
    }
}
