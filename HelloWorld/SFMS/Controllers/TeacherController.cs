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


namespace SFMS.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TeacherController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        public IActionResult List()
        {
            IList<TeacherViewModel> teachers=_applicationDbContext.Teachers.Select(s=>new TeacherViewModel
            {
                Code= s.Code,
                Name= s.Name,
                Email= s.Email,
                Phone= s.Phone,
                Address= s.Address,
                NRC=s.NRC,
                DOB=s.DOB,
                Position= s.Position,
                Id= s.Id,
                Courses=(from tc in _applicationDbContext.TeacherCourses join c in _applicationDbContext.Courses on tc.CourseId equals c.Id
                         where tc.TeacherId==s.Id
                         select new BathViewModel
                         {
                             Name=c.Name,
                             Id=c.Id
                         }).ToList(),
            }).ToList();
            return View(teachers);
        }

        public IActionResult Entry() {
            ViewBag.Courses = _applicationDbContext.Courses.Select(s => new SelectListItem
            {
                Text =s.Name,
                Value = s.Id
            }).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Entry(TeacherViewModel model)
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
                    Teacher teacher = new Teacher()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDte = DateTime.Now,
                        IP= GetLocalIPAddress(),
                        Email = model.Email,
                        Phone = model.Phone,
                        Position = model.Position,
                        Name = model.Name,
                        NRC = model.NRC,
                        Address = model.Address,
                        Code = model.Code,
                        DOB = model.DOB,
                    };
                    _applicationDbContext.Teachers.Add(teacher);
                   int result= _applicationDbContext.SaveChanges();
                    if (courseIds.Count() > 0 && result>0) { //there is courses selected from ui and teacher record is created first.
                        foreach(string courseId in courseIds) {
                            TeacherCourses tc = new TeacherCourses()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreatedDte = DateTime.Now,
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


        public IActionResult Delete(string id) {
            Teacher t = _applicationDbContext.Teachers.Find(id);
            if (t != null) {
                _applicationDbContext.Teachers.Remove(t);//remove the  Teacher record from DBSET
                _applicationDbContext.SaveChanges();//remove effect to the database.
            }
            return RedirectToAction("List");
        }

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
                    Position = s.Position
                }).SingleOrDefault();
            return View(teacherViewModel);
        }

        [HttpPost]
        public IActionResult Edit(TeacherViewModel teacherViewModel) {
            bool isSuccess;
            try {
                Teacher teacher = new Teacher();
                //audit columns
                teacher.Id = teacherViewModel.Id;
                teacher.ModifiedDate = DateTime.Now;
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
