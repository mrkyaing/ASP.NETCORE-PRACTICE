using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMS.Models;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SFMS.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CourseController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public  IActionResult IsCourseAlreadyExists(string Name) {
            if (_applicationDbContext.Courses.Any(x => x.Name.Equals(Name))) {
                return Json(true);
            }
            else {
                return Json($"Name {Name} is already in use.");
            }
        }
            public IActionResult List()
        {
           var courses=_applicationDbContext.Courses.Where(x=>x.IsActive==true).Select(s=>new CourseViewModel
            {
               Name= s.Name,
               Description= s.Description,
                Id= s.Id,
                OpeningDate= s.OpeningDate,
                DurationInHour= s.DurationInHour,
                Fees= s.Fees,
                IsPromotion=s.IsPromotion,
                Fixed=s.Fixed,
                Percetance=s.Percetance,
               FeesAfterPromo=(s.Fees-((s.Fees*s.Percetance)/100)+s.Fixed)
           }).ToList();
            return View(courses);
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Entry() {
            return View(new CourseViewModel());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(CourseViewModel viewModel)
        {
            bool isSuccess = false;
            try {
                if (ModelState.IsValid) {
                    if (_applicationDbContext.Courses.Any(x => x.Name.Equals(viewModel.Name)))
                    {
                        ViewBag.AlreadyExistsMsg = $"{viewModel.Name} is already exists in system.";
                        return View(viewModel);
                    }
                    var  model = new Course(){
                     Id = Guid.NewGuid().ToString(),
                      Name= viewModel.Name,
                      Description= viewModel.Description,
                      OpeningDate= viewModel.OpeningDate,
                      DurationInHour= viewModel.DurationInHour,
                      Fees= viewModel.Fees,
                      IsPromotion=viewModel.IsPromotion,
                      Fixed=viewModel.Fixed,
                      Percetance=viewModel.Percetance
                    };
                    _applicationDbContext.Courses.Add(model);
                    _applicationDbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex) {
            }
            if (isSuccess) {
                TempData["msg"] = "saving success";
            }
            else
                TempData["msg"] = "error occur when saving Course information!!";
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id) {
            var model = _applicationDbContext.Courses.Find(id);
            if (model != null) {
                model.IsActive = false;
                _applicationDbContext.Entry(model).State = EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
                TempData["msg"] = "Delete process successed!!";
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id) {
            var   viewModel= _applicationDbContext.Courses
                .Where(w => w.Id == id)
                .Select(s => new CourseViewModel
                {
                    Id = s.Id,
                   Description= s.Description,
                   Name= s.Name,
                   OpeningDate= s.OpeningDate,
                   DurationInHour= s.DurationInHour,
                   Fees= s.Fees,
                }).SingleOrDefault();
            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(CourseViewModel viewModel) {
            bool isSuccess;
            try {
                var  model = new Course();
                //audit columns
                model.Id = viewModel.Id;
                model.UpdatedAt = DateTime.Now;
                model.IP = GetLocalIPAddress();//calling the method 
                //ui columns
               model.Name=viewModel.Name;
                model.Description = viewModel.Description;
                model.OpeningDate = viewModel.OpeningDate;
                model.DurationInHour = viewModel.DurationInHour;
                model.Fees = viewModel.Fees;
                _applicationDbContext.Entry(model).State = EntityState.Modified;//Updating the existing recrod in db set 
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
