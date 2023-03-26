using Microsoft.AspNetCore.Mvc;
using SFMS.Models.ViewModels;
using SFMS.Models.DAO;
using SFMS.Models;
using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using SFMS.Migrations;

namespace SFMS.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //Constructore Inject Apporach for ApplicationDbContext;
        public AttendanceController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Entry() {
            IList<StudentViewModel> students = _applicationDbContext.Students.Select(b => new StudentViewModel
            {
                Name = b.Name,
                Id = b.Id,
            }).ToList();
            return View(students);
        }

        [HttpPost]
        public  IActionResult Entry(AttendanceViewModel viewModel)
        {
            bool isSuccess=false;
            try {
                Attendance attendance = new Attendance();
                //audit columns
                attendance.Id = Guid.NewGuid().ToString();
                attendance.CreatedDte = DateTime.Now;
                attendance.IP = GetLocalIPAddress();//calling the method 
                 //ui columns
                attendance.AttendaceDate = viewModel.AttendaceDate;
                attendance.InTime = viewModel.InTime;
                attendance.OutTime = viewModel.OutTime;
                attendance.IsLate=string.IsNullOrEmpty(viewModel.IsLate)?false:true;
                attendance.IsLeave = string.IsNullOrEmpty(viewModel.IsLeave) ? false : true;
                attendance.StudentId=viewModel.StudentId;
                _applicationDbContext.Attendances.Add(attendance);//Adding the record Students DBSet
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

        public IActionResult Delete(string id) {
            Student student=_applicationDbContext.Students.Find(id);
            if (student != null) {
                _applicationDbContext.Students.Remove(student);//remove the  student record from DBSET
                _applicationDbContext.SaveChanges();//remove effect to the database.
            }
            return RedirectToAction("List");
        }

        public IActionResult Edit(string id) {
            var attendanceViewModel = _applicationDbContext.Attendances
                .Where(w => w.Id == id)
                .Select(s => new AttendanceViewModel{
                 Id=s.Id,
                 AttendaceDate = s.AttendaceDate,
                 InTime=s.InTime,
                 OutTime=s.OutTime,
                 StudentId=s.StudentId,
                 Student= s.Student,
                 IsLate= s.IsLate == true ? "true" : "false",
                 IsLeave =s.IsLeave==true?"true":"false"}).SingleOrDefault();
            ViewBag.Students = _applicationDbContext.Students.Where(x=>x.Id!=attendanceViewModel.StudentId).Select(s => new SelectListItem
            {
                Value = s.Id,
                Text = s.Name
            }).ToList();
            return View(attendanceViewModel);
        }

        [HttpPost]
        public IActionResult Edit(StudentViewModel studentViewModel) {
            bool isSuccess;
            try {
                Student student = new Student();
                //audit columns
                student.Id=studentViewModel.Id;
                student.ModifiedDate = DateTime.Now;
                student.IP = GetLocalIPAddress();//calling the method 
                //ui columns
                student.Code = studentViewModel.Code;
                student.Name = studentViewModel.Name;
                student.Email = studentViewModel.Email;
                student.Phone = studentViewModel.Phone;
                student.Address = studentViewModel.Address;
                student.NRC = studentViewModel.NRC;
                student.DOB = studentViewModel.DOB;
                student.FatherName = studentViewModel.FatherName;
                student.BathId = studentViewModel.BathId;
                _applicationDbContext.Entry(student).State=EntityState.Modified;//Updating the existing recrod in db set 
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
                TempData["msg"] = "error occur when Updating student information!!";
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
        [HttpPost]
        public IActionResult Update(StudentViewModel studentViewModel)
        {
            bool isUpdateSuccess = false;
            try {
                Student student = new Student();
                //audit columns
                student.ModifiedDate = DateTime.Now;
                student.IP = GetLocalIPAddress();
                //ui columns
                student.Code = studentViewModel.Code;
                student.Name = studentViewModel.Name;
                student.Email = studentViewModel.Email;
                student.Phone = studentViewModel.Phone;
                student.Address = studentViewModel.Address;
                student.NRC = studentViewModel.NRC;
                student.FatherName = studentViewModel.FatherName;
                _applicationDbContext.Entry(student).State = EntityState.Modified;//Updating the record Students DBSet
                _applicationDbContext.SaveChanges();//Upding the record to the database
                isUpdateSuccess = true;
            }
            catch (Exception ex) {

            }
            if (isUpdateSuccess) {
                ViewBag.Msg = "Update success";
            }
            else ViewBag.Msg = "error occur when updating student information!!";
            return View();
        }

    }
}
