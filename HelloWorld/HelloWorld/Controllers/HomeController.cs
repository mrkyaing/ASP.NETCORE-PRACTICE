using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string amOrPm = null;
            int hour = DateTime.Now.Hour;
            if (hour < 12)
            {
                amOrPm = "Good Morning"+DateTime.Now;
            }else
            {
                amOrPm = "Good Afternoon";
            }
            ViewBag.MyTime = amOrPm;
            TempData["myTime"] = amOrPm;
            return View();
        }

        public IActionResult List()
        {
            return View();
        }
        [ActionName("me")]
        public IActionResult doMe()
        {
            TempData["me"] = "Mr Kyaing";
            return View();
        }
        [HttpPost]
        public IActionResult me(string txtemail)
        {
            ViewBag.Msg = txtemail;
           return View();
        }
        [NonAction]
        public int Sum(int n1,int num2)
        {
            return n1 + num2;
        }

        [ActionName("multiple")]
        public IActionResult GetMultiple(int num1,int num2)
        {
            ViewBag.Result=num1* num2;
            return View();
        }
        public string GetMe() => TempData["me"]?.ToString();
    }
}
