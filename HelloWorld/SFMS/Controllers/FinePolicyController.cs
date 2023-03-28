using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMS.Models;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;

namespace SFMS.Controllers
{
    public class FinePolicyController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public FinePolicyController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        public IActionResult List()
        {
           var finePolicies=_applicationDbContext.FinePolicies.Select(s=>new FinePolicyViewModel
            {
               Name= s.Name,
               FineAfterMinutes= s.FineAfterMinutes,
               FineAmount= s.FineAmount,
               Rule= s.Rule,
            }).ToList();
            return View(finePolicies);
        }

        public IActionResult Entry() =>View();
        [HttpPost]
        public IActionResult Entry(FinePolicyViewModel viewModel)
        {
            bool isSuccess = false;
            try {
                if (ModelState.IsValid) {
                    var  model = new FinePolicy(){
                        Id = Guid.NewGuid().ToString(),
                        CreatedDte = DateTime.Now,
                      Name= viewModel.Name,
                    Rule= viewModel.Rule,
                    FineAmount = viewModel.FineAmount,
                    FineAfterMinutes = viewModel.FineAfterMinutes,
                    };
                    _applicationDbContext.FinePolicies.Add(model);
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
                TempData["msg"] = "error occur when saving Fine Policy information!!";
            return RedirectToAction("List");
        }


        public IActionResult Delete(string id) {
            var model = _applicationDbContext.Courses.Find(id);
            if (model != null) {
                _applicationDbContext.Courses.Remove(model);//remove the  record from DBSET
                _applicationDbContext.SaveChanges();//remove effect to the database.
            }
            return RedirectToAction("List");
        }

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

        [HttpPost]
        public IActionResult Edit(CourseViewModel viewModel) {
            bool isSuccess;
            try {
                var  model = new Course();
                //audit columns
                model.Id = viewModel.Id;
                model.ModifiedDate = DateTime.Now;
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
