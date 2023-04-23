using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFMS.Models;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SFMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HomeController(ApplicationDbContext applicationDbContext){
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            ViewBag.TotalStudents = _applicationDbContext.Students.Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
