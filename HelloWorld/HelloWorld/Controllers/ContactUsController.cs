using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
