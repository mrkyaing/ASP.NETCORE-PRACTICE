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
using SFMS.Services;

namespace SFMS.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {

        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

            public IActionResult List() {
            var courses=  _courseService.ReteriveActive();
            return View(courses);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Entry() {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Entry(CourseViewModel viewModel)
        {
            bool isSuccess = false;
            try {
                if (ModelState.IsValid) {
                    //if (_applicationDbContext.Courses.Any(x => x.Name.Equals(viewModel.Name)))
                    //{
                    //    ViewBag.AlreadyExistsMsg = $"{viewModel.Name} is already exists in system.";
                    //    return View(viewModel);
                    //}
                    _courseService.Create(viewModel);
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
            try {
                _courseService.Delete(id);
                TempData["msg"] = "Delete process successed!!";
            }catch(Exception ex) {
                TempData["msg"] = "Error occur when delete recrod!!";
            }
          
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id) {
          var viewModel=_courseService.FindById(id);
            return View(viewModel);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(CourseViewModel viewModel) {
            bool isSuccess;
            try {
                _courseService.Update(viewModel);
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

    }
}
