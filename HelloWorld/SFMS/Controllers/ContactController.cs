using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace SFMS.Controllers {
    public class ContactController : Controller {
       
        private readonly ApplicationDbContext _applicationDbContext;
        public ContactController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult List() {
                IList<ContactAnyQueryViewModel> data = _applicationDbContext.ContactAnyQueries.Where(x=>x.IsActive==true).Select(b => new ContactAnyQueryViewModel
                {
                    Name = b.Name,
                    Id = b.Id,
                     Email = b.Email,
                     Message = b.Message,
                     Subject = b.Subject,
                }).ToList();
                return View(data);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id) {
            var b = _applicationDbContext.ContactAnyQueries.Find(id);
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
