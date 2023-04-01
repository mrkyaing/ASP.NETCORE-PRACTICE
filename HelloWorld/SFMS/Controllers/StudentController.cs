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

        public IActionResult Entry() {
            IList<BatchViewModel> batches = _applicationDbContext.Batches.Select(b => new BatchViewModel
            {
                Name = b.Name,
                Id = b.Id,
            }).ToList();
            return View(batches);
        }

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
                student.BathId= studentViewModel.BathId;
                _applicationDbContext.Students.Add(student);//Adding the record Students DBSet
                _applicationDbContext.SaveChanges();//saving the record to the database
                isSuccess= true;
            }
            catch(Exception ex) {
                isSuccess = false;
            }
            if(isSuccess) {
                TempData["msg"] = "Saving success for "+studentViewModel.Code;
            }
            else
                TempData["msg"] = "Error occur when saving student information!!";

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
              BathName=s.Batch.Name
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
                    FatherName = s.FatherName,
                    BathId=s.BathId,
                    BathName = s.Batch.Name
        }).SingleOrDefault();
            ViewBag.Bathes = _applicationDbContext.Batches.Where(x => x.Id != studentViewModel.BathId)
              .Select(s => new SelectListItem
              {
                  Value = s.Id,
                  Text = s.Name
              }).ToList();
            return View(studentViewModel);
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

    }
}
