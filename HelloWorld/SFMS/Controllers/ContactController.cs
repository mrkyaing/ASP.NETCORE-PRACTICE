using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFMS.Models.DAO;
using SFMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;

using System.Net;

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

        public IActionResult SendEmail(string id) {
            //sending email
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("mr.kyaing7@gmail.com");
            mailMessage.To.Add(id);
            mailMessage.Subject = "Welcome and thanks for your Contact";
            mailMessage.Body = "Our admin will contact you as soon as possiable.We received your contact query information.";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;

            smtpClient.Credentials = new NetworkCredential("mr.kyaing7@gmail.com", "izujavmpvbaualgx");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            try {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Sent Successfully.");
            }
            catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }
            TempData["msg"] = "Email Sent Successfully.";
            return RedirectToAction("List");
        }
    }
}
