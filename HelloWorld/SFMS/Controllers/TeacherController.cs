using Microsoft.AspNetCore.Mvc;
using SFMS.Models;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFMS.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TeacherController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        public IActionResult List()
        {
            IList<TeacherViewModel> teachers=_applicationDbContext.Teachers.Select(s=>new TeacherViewModel
            {
                Code= s.Code,
                Name= s.Name,
                Email= s.Email,
                Phone= s.Phone,
                Address= s.Address,
                NRC=s.NRC,
                DOB=s.DOB,
                Position= s.Position,
            }).ToList();
            return View(teachers);
        }

        public IActionResult Entry() =>View();
        [HttpPost]
        public IActionResult Entry(TeacherViewModel model)
        {
            bool isSuccess = false;
            try {
                if (ModelState.IsValid) {
                    Teacher teacher = new Teacher()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDte = DateTime.Now,
                        Email = model.Email,
                        Phone = model.Phone,
                        Position = model.Position,
                        Name = model.Name,
                        NRC = model.NRC,
                        Address = model.Address,
                        Code = model.Code,
                        DOB = model.DOB,
                    };
                    _applicationDbContext.Teachers.Add(teacher);
                    _applicationDbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception) {
            }
            if (isSuccess) {
                ViewBag.Msg = "saving success";
            }
            else
                ViewBag.Msg = "error occur when saving Teacher information!!";
            return View();
        }
    }
}
