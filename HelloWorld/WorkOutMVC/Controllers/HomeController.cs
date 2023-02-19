using Microsoft.AspNetCore.Mvc;

namespace WorkOutMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        { 
            return View(); 
        }
        public IActionResult Compute()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Me(string firstName,string lastName)
        {
            ViewBag.FullName = $"Hello,My Name is {firstName} {lastName}";
            return View();
        }
        [HttpPost]
        public IActionResult Compute(string fromCurrency, double amount)
        {
            double result = 0;
            ViewBag.selectedfromCurrency = fromCurrency;
            ViewBag.filledAmount = amount;
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
    

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string id,string firstName,string lastName)
        {
            return View();
        }
    }
}
