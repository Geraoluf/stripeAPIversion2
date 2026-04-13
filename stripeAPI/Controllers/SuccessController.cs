using Microsoft.AspNetCore.Mvc;

namespace stripeAPI.Controllers
{
    public class SuccessController : Controller
    {
        public IActionResult Success()
        {
            return View();
        }



        public IActionResult Cancel()
        {
            return View();
        }
    }
}
