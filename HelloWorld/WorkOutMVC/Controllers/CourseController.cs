using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WorkOutMVC.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(IList<string> Id,IList<string> Name,IList<DateTime> StartDate)
        {
            ViewBag.FirstCourseInfo = $"{Name[0]} will start at {StartDate[0]}";
            ViewBag.SecondCourseInfo = $"{Name[1]} will start at {StartDate[1]}";
            return View();
        }
    }
}
