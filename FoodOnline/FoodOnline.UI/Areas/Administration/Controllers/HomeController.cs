using Microsoft.AspNetCore.Mvc;

namespace FoodOnline.UI.Areas.Admin.Controllers
{

    [Area("Administration")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
