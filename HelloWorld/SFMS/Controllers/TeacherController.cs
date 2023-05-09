using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMS.Models;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SFMS.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public TeacherController(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            this._applicationDbContext = applicationDbContext;
            this._userManager = userManager;
        }
        public async Task<IActionResult> List()
        {
            IList<TeacherViewModel> teachers=null;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// will give the user's userId
            var user = await _userManager.FindByIdAsync(userId); // to get current user 
            var role = await _userManager.GetRolesAsync(user); //to get current user's roles
            if (role.Contains("Admin")) 
                teachers = _applicationDbContext.Teachers.Where(x=>x.IsActive==true).Select(s => new TeacherViewModel
                {
                    Code = s.Code,
                    Name = s.Name,
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    NRC = s.NRC,
                    DOB = s.DOB,
                    Position = s.Position,
                    Id = s.Id,
                    UserId=s.UserId,
                    Courses = (from tc in _applicationDbContext.TeacherCourses
                               join c in _applicationDbContext.Courses on tc.CourseId equals c.Id
                               where tc.TeacherId == s.Id && tc.IsActive==true && c.IsActive==true
                               select new CourseViewModel
                               {
                                   Name = c.Name,
                                   Id = c.Id
                               }).ToList()
                }).ToList();
            else
                teachers = _applicationDbContext.Teachers.Where(x => x.UserId.Equals(userId)&& x.IsActive == true).Select(s => new TeacherViewModel
                {
                    Code = s.Code,
                    Name = s.Name,
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    NRC = s.NRC,
                    DOB = s.DOB,
                    Position = s.Position,
                    Id = s.Id,
                    UserId = s.UserId,
                    FacebookUrl=s.FacebookUrl,
                    LinkedinUrl=s.LinkedinUrl,
                    TwitterUrl=s.TwitterUrl,
                    Courses = (from tc in _applicationDbContext.TeacherCourses
                               join c in _applicationDbContext.Courses on tc.CourseId equals c.Id
                               where tc.TeacherId == s.Id && tc.IsActive==true && c.IsActive==true
                               select new CourseViewModel
                               {
                                   Name = c.Name,
                                   Id = c.Id
                               }).ToList()
                }).ToList();
            return View(teachers);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Entry() {
            ViewBag.Courses = _applicationDbContext.Courses.Select(s => new SelectListItem
            {
                Text =s.Name,
                Value = s.Id
            }).ToList();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Entry(TeacherViewModel model)
        {
            string[] courseIds = Request.Form["CourseIds"].ToString().Split(",");
            bool isSuccess = false;
            try {
                if (ModelState.IsValid) {
                    if (_applicationDbContext.Teachers.Any(x => x.Name.Equals(model.Code)))
                    {
                        ViewBag.AlreadyExistsMsg = $"{model.Code} is already exists in system.";
                        return View(model);
                    }
                    var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                    var resultTeacherCreated = await _userManager.CreateAsync(user, "sfms101");//insert the recrod into the database .
                    if (resultTeacherCreated.Succeeded) {
                        //Set the email confirmed directly (it doest not require because of config in middleware .)
                       // await _userManager.IsEmailConfirmedAsync(user);
                       
                        //adding the role Teacher Role when teacher recrod is created.
                        await _userManager.AddToRoleAsync(user, "Teacher");
                     }
                        Teacher teacher = new Teacher(){
                        Id = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.Now,
                        IP= GetLocalIPAddress(),
                        Email = model.Email,
                        Phone = model.Phone,
                        Position = model.Position,
                        Name = model.Name,
                        NRC = model.NRC,
                        Address = model.Address,
                        Code = model.Code,
                        DOB = model.DOB,
                        UserId=user.Id,
                            FacebookUrl = model.FacebookUrl,
                            LinkedinUrl = model.LinkedinUrl,
                            TwitterUrl = model.TwitterUrl,
                        };
                    _applicationDbContext.Teachers.Add(teacher);
                   int result= _applicationDbContext.SaveChanges();
                    if (courseIds.Count() > 0 && result>0) { //there is courses selected from ui and teacher record is created first.
                        foreach(string courseId in courseIds) {
                            TeacherCourses tc = new TeacherCourses()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreatedAt = DateTime.Now,
                                IP = GetLocalIPAddress(),
                                CourseId = courseId,
                                TeacherId = teacher.Id
                            };
                           _applicationDbContext.TeacherCourses.Add(tc);
                            _applicationDbContext.SaveChanges();
                        }
                    }
                    isSuccess = true;
                }
            }
            catch (Exception) {
            }
            if (isSuccess) {
                TempData["msg"] = "Saving success for " + model.Code;
            }
            else
                TempData["msg"]= "error occur when saving Teacher information!!";
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id) {
            Teacher t = _applicationDbContext.Teachers.Find(id);
            if (t != null) {
                t.IsActive = false;
                _applicationDbContext.Entry(t).State = EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
                TempData["msg"] = "Delete process successed!!";
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id) {
            TeacherViewModel teacherViewModel = _applicationDbContext.Teachers
                .Where(w => w.Id == id)
                .Select(s => new TeacherViewModel
                {
                    Id = s.Id,
                    Code = s.Code,
                    Name = s.Name,
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    NRC = s.NRC,
                    DOB = s.DOB,
                    Position = s.Position,
                    UserId=s.UserId
                }).SingleOrDefault();
            return View(teacherViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(TeacherViewModel teacherViewModel) {
            bool isSuccess;
            try {
                Teacher teacher = new Teacher();
                //audit columns
                teacher.Id = teacherViewModel.Id;
                teacher.UpdatedAt = DateTime.Now;
                teacher.IP = GetLocalIPAddress();//calling the method 
                //ui columns
                teacher.Code = teacherViewModel.Code;
                teacher.Name = teacherViewModel.Name;
                teacher.Email = teacherViewModel.Email;
                teacher.Phone = teacherViewModel.Phone;
                teacher.Address = teacherViewModel.Address;
                teacher.NRC = teacherViewModel.NRC;
                teacher.DOB = teacherViewModel.DOB;
                teacher.Position = teacherViewModel.Position;
                teacher.UserId = teacherViewModel.UserId;
                _applicationDbContext.Entry(teacher).State = EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
                isSuccess = true;
            }
            catch (Exception ex) {
                isSuccess = false;
            }
            if (isSuccess) {
                TempData["msg"] = "Update success for "+teacherViewModel.Code;
            }
            else
                TempData["msg"] = "error occur when Updating the record!!";
            return RedirectToAction("List");
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
