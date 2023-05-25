using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace SFMS.Controllers {
    [Authorize(Roles = "Admin")]
    public class NewStudentRegisterController : Controller {
       
        private readonly ApplicationDbContext _applicationDbContext;
        public NewStudentRegisterController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public IActionResult List() {
                IList<NewStudentRegisterViewModel> data = _applicationDbContext.NewStudentRegisters.Where(x=>x.IsActive==true).Select(b => new NewStudentRegisterViewModel
                {
                    Name = b.Name,
                    Id = b.Id,
                     Email = b.Email,
                     Phone = b.Phone,
                     Remark = b.Remark,
                     CourseId = b.Course.Name
                }).ToList();
                return View(data);
        }
        public IActionResult Delete(string id) {
            var b = _applicationDbContext.NewStudentRegisters.Find(id);
            if (b != null) {
                b.IsActive = false;
                _applicationDbContext.Entry(b).State = EntityState.Modified;//Updating the existing recrod in db set 
                _applicationDbContext.SaveChanges();//Updating  the record to the database
                TempData["msg"] = "Delete process successed!!";
            }
            return RedirectToAction("List");
        }
    }
}
