using Microsoft.AspNetCore.Mvc;

namespace Front.MVC.ViewComponents
{
    public class SummaryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
