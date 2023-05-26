using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SFMS.Models;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Mail;

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
            ViewBag.TotalTeachers = _applicationDbContext.Teachers.Count();
            ViewBag.TotalNewStudentRegister= _applicationDbContext.NewStudentRegisters.Count();
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactAnyQueryViewModel viewModel) {
            if (ModelState.IsValid) {
                var ContactAnyQuery = new ContactAnyQuery()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    Subject = viewModel.Subject,
                    IP = GetLocalIPAddress(),
                    Message = viewModel.Message
                };
                _applicationDbContext.ContactAnyQueries.Add(ContactAnyQuery);
                _applicationDbContext.SaveChanges();

                ViewBag.Message = "Send your message to the system administrator.Thanks for your message.";
            }
            else {
                ViewBag.Message = "Oh sorry,we face some issues when you send your message to us.";
            }
            return View();
        }
        public IActionResult About() {
            var courses = GetCourses();
            return View(courses);
        }
        private IList<CourseViewModel> GetCourses() {
            IList<CourseViewModel> courses = _applicationDbContext.Courses.Where(x=>x.IsActive==true).Select(c => new CourseViewModel
            {
                Name = c.Name,
                Id = c.Id,
            }).ToList();
            return courses;
        }
        public IActionResult NewStudentRegister(NewStudentRegisterViewModel viewModel) {
            if (ModelState.IsValid) {
                var newStudentRegister = new NewStudentRegister()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    Phone = viewModel.Phone,
                    IP = GetLocalIPAddress(),
                    Remark = viewModel.Remark,
                   CourseId = viewModel.CourseId
                };
                _applicationDbContext.NewStudentRegisters.Add(newStudentRegister);
                _applicationDbContext.SaveChanges();
                ViewBag.Message = "Send your message to the system administrator.Thanks for your message.";
            }
            else {
                ViewBag.Message = "Oh sorry,we face some issues when you send your message to us.";
            }
            var courses = GetCourses();
            return View("About", courses);
        }
        public IActionResult CourseDetail(string courseId) {
           CourseViewModel course=_applicationDbContext.Courses.Where(x=>x.IsActive==true&&x.Id==courseId).Select(s=>new CourseViewModel
           {
               Name = s.Name,
               Description = s.Description,
               Id = s.Id,
               OpeningDate = s.OpeningDate,
               DurationInHour = s.DurationInHour,
               Fees = s.Fees,
               IsPromotion = s.IsPromotion,
               Fixed = s.Fixed,
               Percetance = s.Percetance,
               FeesAfterPromo = (s.Fees - ((s.Fees * s.Percetance) / 100) + s.Fixed)
           }).FirstOrDefault();
            return View(course);
        }
        public IActionResult Teachers() {
            ViewBag.Teachers = _applicationDbContext.Teachers.Where(x => x.IsActive == true).ToList();
            return View();
        }
        public IActionResult Courses() {
            ViewBag.PromotionCourses= _applicationDbContext.Courses.Where(x=>x.IsPromotion==true && x.IsActive==true).ToList();
            ViewBag.PopularCourses = _applicationDbContext.Courses.Where(x=>x.IsActive == true &&x.Batches.Where(y=>y.IsActive==true).Count()>=5).ToList();
            ViewBag.AllCourses = _applicationDbContext.Courses.Where(x => x.IsActive == true).ToList();
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// will give the user's userId
            IList<FineTransactionViewModel> studentFine = (from ft in _applicationDbContext.FineTransactions
                               join s in _applicationDbContext.Students
                               on ft.StudentId equals s.Id
                               where s.UserId == userId && s.IsActive==true && ft.IsActive==true
                               select new FineTransactionViewModel
                               {
                                   FinedDate = ft.FinedDate,
                                   InTime = ft.InTime,
                                   OutTime = ft.OutTime,
                                   FineAmount = ft.FineAmount,
                                   FinePolicyId = ft.FinePolicyId,
                                   FinePolicy = ft.FinePolicy
                               }).ToList();
            return View(studentFine);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private static string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
