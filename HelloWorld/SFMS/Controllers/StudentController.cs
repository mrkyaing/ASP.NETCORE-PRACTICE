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

namespace SFMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //Constructore Inject Apporach for ApplicationDbContext;
        public StudentController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Entry()=>View();

        [HttpPost]
        public  IActionResult Entry(StudentViewModel studentViewModel)
        {
            bool isSuccess;
            try {
                Student student = new Student();
                //audit columns
                student.Id = Guid.NewGuid().ToString();
                student.CreatedDte = DateTime.Now;
                student.IP = GetLocalIPAddress();//calling the method 
                //ui columns
                student.Code = studentViewModel.Code;
                student.Name = studentViewModel.Name;
                student.Email = studentViewModel.Email;
                student.Phone = studentViewModel.Phone;
                student.Address = studentViewModel.Address;
                student.NRC = studentViewModel.NRC;
                student.DOB=studentViewModel.DOB;
                student.FatherName = studentViewModel.FatherName;
                _applicationDbContext.Students.Add(student);//Adding the record Students DBSet
                _applicationDbContext.SaveChanges();//saving the record to the database
                isSuccess= true;
            }
            catch(Exception ex) {
                isSuccess = false;
            }
            if(isSuccess) {
                ViewBag.Msg = "saving success";
            }
            else 
                ViewBag.Msg = "error occur when saving student information!!";

            return RedirectToAction("List");
        }//end of entry post method
        

        public IActionResult List()
        {
          IList<StudentViewModel> students= _applicationDbContext.Students.Select
                (s=>new StudentViewModel{
               Id=s.Id,
              Code=s.Code,
              Name=s.Name,
              Email=s.Email,
              Address=s.Address,
              Phone=s.Phone,
              FatherName=s.FatherName,
              NRC=s.NRC,
              DOB = s.DOB,
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
            StudentViewModel studentViewModel = _applicationDbContext.Students
                .Where(w => w.Id == id)
                .Select(s => new StudentViewModel
                {
                      Id=s.Id,
                     Code = s.Code,
                    Name = s.Name,
                    Email = s.Email,
                    Phone = s.Phone,
                    Address = s.Address,
                    NRC = s.NRC,
                    DOB = s.DOB,
                    FatherName = s.FatherName
        }).SingleOrDefault();
            return View(studentViewModel);
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

        //[HttpPost]
        //public IActionResult Delete(string Id)
        //{
        //    bool isDeleteSuccess = false;
        //    try {
        //        var student = _applicationDbContext.Students.Find(Id);
        //        if (student != null) {
        //            _applicationDbContext.Entry(student).State = EntityState.Deleted;//Remove the record Students DBSet
        //            _applicationDbContext.SaveChanges();//Deleting the record to the database
        //            isDeleteSuccess = true;
        //        }
        //        else
        //            isDeleteSuccess = false;
        //    }
        //    catch (Exception ex) {

        //    }
        //    if (isDeleteSuccess) {
        //        ViewBag.Msg = "Delete success";
        //    }
        //    else ViewBag.Msg = "error occur when deleting student information!!";
        //    return View();
        //}
    }
}
