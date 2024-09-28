using Microsoft.AspNetCore.Mvc;
using Front.MVC.Models;

namespace Front.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("error/{id:length(3,3)}")]
        public IActionResult Error(string id)
        {
            var modelError = new ErrorViewModel();

            if (id == "500")
            {
                modelError.ErrorCode = id;
                modelError.Title = "Error Server";
                modelError.Message = "Error Server";
            }
            else if (id == "404")
            {
                modelError.ErrorCode = id;
                modelError.Title = "Not Found";
                modelError.Message = "Not Found";
            }
            else if (id == "403")
            {
                modelError.ErrorCode = id;
                modelError.Title = "Access Denied";
                modelError.Message = "Access Denied";
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelError);
        }

        [HttpGet("system-unavailable")]
        public IActionResult SystemUnavailable(string id)
        {
            var modelError = new ErrorViewModel();

            modelError.ErrorCode = id;
            modelError.Title = "system unavailable";
            modelError.Message = "System temporarily unavailable. This can occur when users are overloaded.";

            return View("Error", modelError);
        }
    }
}
