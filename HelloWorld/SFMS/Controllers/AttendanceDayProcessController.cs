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
using static System.Net.WebRequestMethods;

namespace SFMS.Controllers
{
    public class AttendanceDayProcessController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //Constructore Inject Apporach for ApplicationDbContext;
        public AttendanceDayProcessController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult DayEndProcess() {
          
            IList<StudentViewModel> students = _applicationDbContext.Students.Select(b => new StudentViewModel
            {
                Name = b.Name,
                Id = b.Id,
            }).ToList();
            IList<BatchViewModel> batches = _applicationDbContext.Batches.Select(b => new BatchViewModel
            {
                Name = b.Name,
                Id = b.Id,
            }).ToList();
            AttendanceDayEndProcessViewModel attendanceDayEndProcessViewModel=new AttendanceDayEndProcessViewModel(){
                students=students,
                batches= batches
            };

            return View(attendanceDayEndProcessViewModel);
        }

        [HttpPost]
        public  IActionResult DayEndProcess(AttendanceDayEndProcessViewModel viewModel)
        {
            bool isSuccess=false;
            try {
                var ftsCheckBeforeDayEndProcess = _applicationDbContext.FineTransactions.Where(x => x.StudentId == viewModel.StudentId && (x.FinedDate >= viewModel.FromDayEndDate && x.FinedDate <= viewModel.ToDayEndDate)).ToList();
                _applicationDbContext.FineTransactions.RemoveRange();
                _applicationDbContext.SaveChanges();//saving the record to the database
                IList<FineTransaction> fts = new List<FineTransaction>(ftsCheckBeforeDayEndProcess);
                if (viewModel.StudentId != null) {
                    var finePolicy = _applicationDbContext.FinePolicies.SingleOrDefault();
                    var attendances = _applicationDbContext.Attendances.Where(x=>x.StudentId==viewModel.StudentId && x.IsLate==true && (x.AttendaceDate>=viewModel.FromDayEndDate && x.AttendaceDate<=viewModel.ToDayEndDate)).ToList();
                    foreach(var item in attendances) {
                        TimeSpan inTime=TimeSpan.Parse(item.InTime);
                        if (inTime.Minutes > finePolicy.FineAfterMinutes) {
                            FineTransaction ft = new FineTransaction()
                            {
                                //audit columns
                                Id = Guid.NewGuid().ToString(),
                              CreatedDte = DateTime.Now,
                            IP = GetLocalIPAddress(),//calling the method 
                            FinedDate = item.AttendaceDate,
                                StudentId = item.StudentId,
                                FinePolicyId = finePolicy.Id,
                                InTime=item.InTime,
                                OutTime=item.OutTime,
                                FineAmount=finePolicy.FineAmount
                            };
                            fts.Add(ft);
                        }
                    }
                }
                _applicationDbContext.FineTransactions.AddRange(fts);//Adding the record Students DBSet
                _applicationDbContext.SaveChanges();//saving the record to the database
                isSuccess= true;
            }
            catch(Exception ex) when(ex.InnerException.Message.Contains("Cannot insert duplicate key row in object")){
                TempData["msg"] = $"Attendance date {viewModel.FromDayEndDate} is already exists in system.";
                return RedirectToAction("List");
            }
            catch(Exception e) {

            }
            if(isSuccess) {
                TempData["msg"] = $"Saving Day End Process from {viewModel.FromDayEndDate} to {viewModel.ToDayEndDate}";
            }
            else
                TempData["msg"] = "Error occur when processing Day End !!";

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
