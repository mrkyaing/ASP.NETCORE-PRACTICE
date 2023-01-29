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
                amOrPm = "Good Morning";
            }else
            {
                amOrPm = "Good Afternoon";

            }
            ViewBag.MyTime = amOrPm;
            return View();
        }

        public IActionResult List()
        {
            return View();
        }
    }
}
