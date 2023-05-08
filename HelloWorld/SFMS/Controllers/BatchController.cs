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
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SFMS.Controllers
{
    [Authorize]
    public class BatchController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BatchController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        public IActionResult List()
        {
            IList<BatchViewModel> batches=_applicationDbContext.Batches.Where(x=>x.IsActive==true).Select(b=>new BatchViewModel
            {
               Name= b.Name,
               Description= b.Description,
                Id= b.Id,
                CourseName=b.Course.Name
            }).ToList();
            return View(batches);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Entry() {
            IList<CourseViewModel> coursesViewModel = _applicationDbContext.Courses.Where(x => x.IsActive == true).Select(s => new CourseViewModel
            {
                Name = s.Name,
                Id = s.Id,
            }).ToList();
            return View(coursesViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(BatchViewModel viewModel)
        {
            bool isSuccess = false;
            try {
                if (ModelState.IsValid) {
                    if (_applicationDbContext.Batches.Where(x=>x.IsActive==true).Any(x => x.Name.Equals(viewModel.Name)))
                    {
                        ViewBag.AlreadyExistsMsg = $"{viewModel.Name} is already exists in system.";
                        return View(viewModel);
                    }
                    var  b = new Batch()
                    {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    Name= viewModel.Name,
                    Description= viewModel.Description,
                    CourseId=viewModel.CourseId
                    };
                    _applicationDbContext.Batches.Add(b);
                    _applicationDbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception) {
            }
            if (isSuccess) {
                TempData["msg"] = "saving success";
            }
            else
                TempData["msg"] = "error occur when saving Batch information!!";
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id) {
            var b = _applicationDbContext.Batches.Find(id);
            if (b != null) {
                b.IsActive = false;
                _applicationDbContext.Entry(b).State = EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
                TempData["msg"] = "Delete process successed!!";
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id) {
            var   batchViewModel= _applicationDbContext.Batches
                .Where(w => w.Id == id && w.IsActive==true)
                .Select(s => new BatchViewModel
                {
                    Id = s.Id,
                   Description= s.Description,
                   Name= s.Name,
                   CourseId= s.CourseId,
                   CourseName=s.Course.Name
                }).SingleOrDefault();
            ViewBag.Courses= _applicationDbContext.Courses.Where(x=>x.Id!= batchViewModel.CourseId && x.IsActive==true)
                .Select(s => new SelectListItem{
                Value = s.Id,
                Text = s.Name
            }).ToList();
            return View(batchViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(BatchViewModel viewModel) {
            bool isSuccess;
            try {
                var  b = new Batch();
                //audit columns
                b.Id = viewModel.Id;
                b.UpdatedAt = DateTime.Now;
                b.IP = GetLocalIPAddress();//calling the method 
                //ui columns
               b.Name=viewModel.Name;
                b.Description = viewModel.Description;
                b.CourseId = viewModel.CourseId;
                _applicationDbContext.Entry(b).State = EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
                isSuccess = true;
            }
            catch (Exception ex) {
                isSuccess = false;
            }
            if (isSuccess) {
                TempData["msg"] = "Update success for "+viewModel.Name;
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
