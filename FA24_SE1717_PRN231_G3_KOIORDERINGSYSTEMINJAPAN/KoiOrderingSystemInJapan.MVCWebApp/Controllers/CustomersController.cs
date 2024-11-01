using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
