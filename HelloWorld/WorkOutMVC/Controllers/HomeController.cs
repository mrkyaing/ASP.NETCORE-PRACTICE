using Microsoft.AspNetCore.Mvc;

namespace WorkOutMVC.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Compute()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Compute(string fromCurrency, double amount)
        {
            double result = 0;
            if (fromCurrency.Equals("usd"))
                result = amount * 2100;
            else if (fromCurrency.Equals("sgd"))
                result = amount * 2000;
            else if (fromCurrency.Equals("thb"))
                result = amount * 80;
            else if (fromCurrency.Equals("chy"))
                result = amount * 500;
            ViewBag.CurrentResult = result;
            return View();
        }
    }
}
