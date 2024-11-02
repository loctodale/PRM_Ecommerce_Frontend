using KoiOrderingSystemInJapan.Data.Request.Payments;
using Microsoft.AspNetCore.Mvc;

namespace KoiOrderingSystemInJapan.MVCWebApp.Controllers
{
    public class PaymentCallBackController : Controller
    {
        public IActionResult Index(PaymentResponse response)
        {
            return View(response);
        }
    }
}
