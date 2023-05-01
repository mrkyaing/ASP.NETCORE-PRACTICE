using Microsoft.AspNetCore.Mvc;
using SFMS.Models.ViewModels;
using SFMS.Models.DAO;
using SFMS.Models;
using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace SFMS.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AttendanceController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Entry() {
            IList<StudentViewModel> students = _applicationDbContext.Students.Select(b => new StudentViewModel
            {
                Name = b.Name,
                Id = b.Id,
            }).ToList();
            return View(students);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public  IActionResult Entry(AttendanceViewModel viewModel)
        {
            bool isSuccess=false;
            try {
                Attendance attendance = new Attendance();
                //audit columns
                attendance.Id = Guid.NewGuid().ToString();
                attendance.CreatedAt = DateTime.Now;
                attendance.IP = GetLocalIPAddress();//calling the method 
                 //ui columns
                attendance.AttendaceDate = viewModel.AttendaceDate;
                attendance.InTime = viewModel.InTime;
                attendance.OutTime = viewModel.OutTime;             
                attendance.IsLate=string.IsNullOrEmpty(viewModel.IsLate)?false:true;
                attendance.IsLeave = string.IsNullOrEmpty(viewModel.IsLeave) ? false : true;
                attendance.StudentId=viewModel.StudentId;
                _applicationDbContext.Attendances.Add(attendance);//Adding the record  DBSet
                _applicationDbContext.SaveChanges();//saving the record to the database
                isSuccess= true;
            }
            catch(Exception ex) when(ex.InnerException.Message.Contains("Cannot insert duplicate key row in object")){
                TempData["msg"] = $"Attendance date {viewModel.AttendaceDate} for student code {_applicationDbContext.Students.Find(viewModel.StudentId).Code} is already exists in system.";
                return RedirectToAction("List");
            }
            catch(Exception e) {

            }
            if(isSuccess) {
                TempData["msg"] = $"Saving success for studentId:{_applicationDbContext.Students.Find(viewModel.StudentId).Name} on {viewModel.AttendaceDate}";
            }
            else
                TempData["msg"] = "Error occur when saving attendance information!!";

            return RedirectToAction("List");
        }//end of entry post method
        
        public IActionResult List()
        {
          IList<AttendanceViewModel> students= _applicationDbContext.Attendances.Select
                (s=>new AttendanceViewModel {
                Id=s.Id,
                AttendaceDate=s.AttendaceDate,
                InTime=s.InTime,
                OutTime=s.OutTime,
                IsLeave=s.IsLeave==true?"Leave":null,
                IsLate= s.IsLate == true ? "Late" : null,
                Student=s.Student
                }).ToList();
            return View(students);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id) {
            var model=_applicationDbContext.Attendances.Find(id);
            if (model != null) {
                _applicationDbContext.Attendances.Remove(model);//remove the  student record from DBSET
                _applicationDbContext.SaveChanges();//remove effect to the database.
                TempData["msg"] = $"Deleted successed.";
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id) {
            var attendanceViewModel = _applicationDbContext.Attendances
                .Where(w => w.Id == id)
                .Select(s => new AttendanceViewModel
                {
                    Id = s.Id,
                    AttendaceDate = s.AttendaceDate,
                    InTime = s.InTime,
                    OutTime = s.OutTime,
                    StudentId = s.StudentId,
                    Student = s.Student,
                    IsLate =Convert.ToString(s.IsLate),
                    IsLeave = Convert.ToString(s.IsLeave)
                }).SingleOrDefault();
            ViewBag.Students = _applicationDbContext.Students.Where(x=>x.Id!=attendanceViewModel.StudentId).Select(s => new SelectListItem
            {
                Value = s.Id,
                Text = s.Name
            }).ToList();
            return View(attendanceViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(AttendanceViewModel viewModel) {
            bool isSuccess;
            try {
                Attendance attendance = new Attendance();
                //audit columns
                attendance.Id= viewModel.Id;
                attendance.UpdatedAt = DateTime.Now;
                attendance.IP = GetLocalIPAddress();//calling the method 
                //ui columns
                attendance.AttendaceDate = viewModel.AttendaceDate;
                attendance.InTime = viewModel.InTime;
                attendance.OutTime = viewModel.OutTime;
                attendance.IsLate = string.IsNullOrEmpty(viewModel.IsLate) ? false : true;
                attendance.IsLeave = string.IsNullOrEmpty(viewModel.IsLeave) ? false : true;
                attendance.StudentId = viewModel.StudentId;
                _applicationDbContext.Entry(attendance).State=EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
                isSuccess = true;
            }
            catch (Exception ex) {
                isSuccess = false;
            }
            if (isSuccess) {
                TempData["msg"]= "Update success";
            }
            else
                TempData["msg"] = "error occur when Updating attendance information!!";
            return RedirectToAction("List");
        }

        //finding the local ip in your machine
        private static string GetLocalIPAddress()
        {
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
