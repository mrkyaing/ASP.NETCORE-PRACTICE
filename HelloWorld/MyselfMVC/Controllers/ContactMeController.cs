using Microsoft.AspNetCore.Mvc;
using MyselfMVC.Models;

namespace MyselfMVC.Controllers
{
    public class ContactMeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(ContactMe contactMe)
        {
            ViewBag.ContactMe = $"Thanks {contactMe.Name} for your message .I will contact you ASAP.";
            return View();
        }
    }
}
