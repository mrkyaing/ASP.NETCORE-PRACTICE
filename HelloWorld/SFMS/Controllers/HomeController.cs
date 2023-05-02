using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager) {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.TotalStudents = _applicationDbContext.Students.Count();
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About() {
            return View();
        }

        public IActionResult Teachers() {
            ViewBag.Teachers = _applicationDbContext.Teachers.ToList();
            return View();
        }
        public IActionResult Courses() {
            ViewBag.PromotionCourses= _applicationDbContext.Courses.Where(x=>x.IsPromotion==true).ToList();
            ViewBag.PopularCourses = _applicationDbContext.Courses.Where(x=>x.Batches.Count()>=5).ToList();
            ViewBag.AllCourses = _applicationDbContext.Courses.ToList();
            return View();
        }
        public IActionResult StudentProfile() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// will give the user's userId
            IList<StudentViewModel> students = _applicationDbContext.Students.Where(x => x.UserId.Equals(userId)).Select
               (s => new StudentViewModel
               {
                   Id = s.Id,
                   Code = s.Code,
                   Name = s.Name,
                   Email = s.Email,
                   Address = s.Address,
                   Phone = s.Phone,
                   FatherName = s.FatherName,
                   NRC = s.NRC,
                   DOB = s.DOB,
                   BathName = s.Batch.Name
               }).ToList();
            return View(students);
        }
        public IActionResult StudentAttendance() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// will give the user's userId
            IList<AttendanceViewModel> attendancesOfStudent = (from a in _applicationDbContext.Attendances
                                                               join s in _applicationDbContext.Students
                                                               on a.StudentId equals s.Id
                                                               where s.UserId.Equals(userId)
                                                               select new AttendanceViewModel
                                                               {
                                                                   AttendaceDate = a.AttendaceDate,
                                                                   InTime = a.InTime,
                                                                   OutTime = a.OutTime,
                                                                   IsLeave = a.IsLeave == true ? "Leave" : null,
                                                                   IsLate = a.IsLate == true ? "Late" : null,
                                                               }).ToList();
            return View(attendancesOfStudent);
          
        }
        public IActionResult StudentFines() {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
