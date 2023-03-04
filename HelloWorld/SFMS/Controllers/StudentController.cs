using Microsoft.AspNetCore.Mvc;
using SFMS.Models.ViewModels;
using SFMS.Models.DAO;
using SFMS.Models;
using System;
using System.Transactions;
using System.Net;

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

        public IActionResult Entry()=>View();

        [HttpPost]
        public  IActionResult Entry(StudentViewModel studentViewModel)
        {
            bool isSuccess = false;
            try {
                Student student = new Student();
                //audit columns
                student.Id = Guid.NewGuid().ToString();
                student.CreatedDte = DateTime.Now;
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                student.IP = myIP;
                //ui columns
                student.Code = studentViewModel.Code;
                student.Name = studentViewModel.Name;
                student.Email = studentViewModel.Email;
                student.Phone = studentViewModel.Phone;
                student.Address = studentViewModel.Address;
                student.NRC = studentViewModel.NRC;
                student.FatherName = studentViewModel.FatherName;
                _applicationDbContext.Students.Add(student);//Adding the record Students DBSet
                _applicationDbContext.SaveChanges();//saving the record to the database
                isSuccess= true;
            }
            catch(Exception ex) {
              
            }
            if(isSuccess) {
                ViewBag.Msg = "saving success";
            }
            else ViewBag.Msg = "error occur when saving student information!!";
            return View();
        }
    }
}
