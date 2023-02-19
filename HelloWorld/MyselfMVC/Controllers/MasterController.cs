using Microsoft.AspNetCore.Mvc;

namespace MyselfMVC.Controllers
{
    public class MasterController : Controller
    {
        public IActionResult Master()
        {
            return View();
        }
    }
}
