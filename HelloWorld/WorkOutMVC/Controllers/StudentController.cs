using Microsoft.AspNetCore.Mvc;
using WorkOutMVC.Models;

namespace WorkOutMVC.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Student student)
        {
            ViewBag.StudentInfo = $"Registration successed with {student.Id} and {student.FirstName} {student.LastName} at City {student.HomeAddress.City}";
            return View();
        }
    }
}
