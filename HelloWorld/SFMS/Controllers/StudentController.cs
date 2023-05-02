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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using System.Threading.Tasks;

namespace SFMS.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public StudentController(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Entry() {
            IList<BatchViewModel> batches = _applicationDbContext.Batches.Select(b => new BatchViewModel
            {
                Name = b.Name,
                Id = b.Id,
            }).ToList();
            return View(batches);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Entry(StudentViewModel studentViewModel)
        {
            try {
                if (ModelState.IsValid)
                {
                    if (_applicationDbContext.Teachers.Any(x => x.Name.Equals(studentViewModel.Code)))
                    {
                        ViewBag.AlreadyExistsMsg = $"{studentViewModel.Code} is already exists in system.";
                        return View(studentViewModel);
                    }
                    var user = new IdentityUser { UserName = studentViewModel.Email, Email = studentViewModel.Email };
                    var result = await _userManager.CreateAsync(user, "sfms101");//insert the recrod into the database .
                    if (result.Succeeded) {
                     //Set the email confirmed directly (it doest not require because of config in middleware .)
                     // await _userManager.IsEmailConfirmedAsync(user);
                     
                    //adding the role STUDENT role when student recrod is created.
                     await _userManager.AddToRoleAsync(user, "Student");
                    //creating the student record 
                    Student student = new Student();
                    //audit columns
                    student.Id = Guid.NewGuid().ToString();
                    student.CreatedAt = DateTime.Now;
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
                    student.UserId = user.Id;//for identity user
                    _applicationDbContext.Students.Add(student);//Adding the record Students DBSet
                    _applicationDbContext.SaveChanges();//saving the record to the database
                     TempData["msg"] = "Saving success for " + studentViewModel.Code +" and create user for student with default password.";
                    }
                }             
            }
            catch(Exception ex) {
                TempData["msg"] = "Error occur when saving student information!!";
            }        
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id) {
            Student student=_applicationDbContext.Students.Find(id);
            if (student != null) {
                _applicationDbContext.Students.Remove(student);//remove the  student record from DBSET
                _applicationDbContext.SaveChanges();//remove effect to the database.
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(StudentViewModel studentViewModel) {
            bool isSuccess;
            try {
                Student student = new Student();
                //audit columns
                student.Id=studentViewModel.Id;
                student.UpdatedAt = DateTime.Now;
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
