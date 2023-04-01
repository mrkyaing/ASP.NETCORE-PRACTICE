using Microsoft.AspNetCore.Mvc;
using SFMS.Models.ViewModels;
using SFMS.Models.DAO;
using SFMS.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;


namespace SFMS.Controllers
{
    public class AttendanceDayProcessController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //Constructore Inject Apporach for ApplicationDbContext;
        public AttendanceDayProcessController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult DayEndProcess() {
          
            IList<StudentViewModel> students = _applicationDbContext.Students.Select(b => new StudentViewModel
            {
                Name = b.Name,
                Id = b.Id,
            }).ToList();
            IList<BatchViewModel> batches = _applicationDbContext.Batches.Select(b => new BatchViewModel
            {
                Name = b.Name,
                Id = b.Id,
            }).ToList();
            AttendanceDayEndProcessViewModel attendanceDayEndProcessViewModel=new AttendanceDayEndProcessViewModel(){
                students=students,
                batches= batches
            };
            return View(attendanceDayEndProcessViewModel);
        }

        [HttpPost]
        public  IActionResult DayEndProcess(AttendanceDayEndProcessViewModel viewModel)
        {
            var finePolicy = _applicationDbContext.FinePolicies.SingleOrDefault();
            bool isSuccess=false;
            try {
                var ftsCheckBeforeDayEndProcess = _applicationDbContext.FineTransactions.Where(x => x.StudentId == viewModel.StudentId && (x.FinedDate >= viewModel.FromDayEndDate && x.FinedDate <= viewModel.ToDayEndDate)).ToList();
                _applicationDbContext.FineTransactions.RemoveRange();
                _applicationDbContext.SaveChanges();//saving the record to the database
                IList<FineTransaction> fts = new List<FineTransaction>(ftsCheckBeforeDayEndProcess);
                if (!viewModel.StudentId.Equals("so")) {
                 
                    var attendances = _applicationDbContext.Attendances.Where(x=>x.StudentId==viewModel.StudentId && x.IsLate==true && (x.AttendaceDate>=viewModel.FromDayEndDate && x.AttendaceDate<=viewModel.ToDayEndDate)).ToList();
                    foreach(var item in attendances) {
                        TimeSpan inTime=TimeSpan.Parse(item.InTime);
                        if (inTime.Minutes > finePolicy.FineAfterMinutes) {
                            FineTransaction ft = new FineTransaction()
                            {
                                //audit columns
                                Id = Guid.NewGuid().ToString(),
                               CreatedDte = DateTime.Now,
                               IP = GetLocalIPAddress(),//calling the method 
                                FinedDate = item.AttendaceDate,
                                StudentId = item.StudentId,
                                FinePolicyId = finePolicy.Id,
                                InTime=item.InTime,
                                OutTime=item.OutTime,
                                FineAmount=finePolicy.FineAmount
                            };
                            fts.Add(ft);
                        }
                    }
                }
                if (!viewModel.BathId.Equals("so")) {
                    var attendances = (from a in _applicationDbContext.Attendances
                                                join s in _applicationDbContext.Students
                                                on a.StudentId equals s.Id
                                                join b in _applicationDbContext.Batches
                                                on s.BathId equals b.Id
                                                where s.BathId.Equals(viewModel.BathId) && a.AttendaceDate>=viewModel.FromDayEndDate && a.AttendaceDate<=viewModel.ToDayEndDate
                                                select new AttendanceViewModel
                                                {
                                                    AttendaceDate=a.AttendaceDate,
                                                    StudentId=s.Id,
                                                    InTime=a.InTime,
                                                    OutTime=a.OutTime,
                                                }).ToList();
                    foreach (var item in attendances) {
                        TimeSpan inTime = TimeSpan.Parse(item.InTime);
                        if (inTime.Minutes > finePolicy.FineAfterMinutes) {
                            FineTransaction ft = new FineTransaction()
                            {
                                //audit columns
                                Id = Guid.NewGuid().ToString(),
                                CreatedDte = DateTime.Now,
                                IP = GetLocalIPAddress(),//calling the method 
                                FinedDate = item.AttendaceDate,
                                StudentId = item.StudentId,
                                FinePolicyId = finePolicy.Id,
                                InTime = item.InTime,
                                OutTime = item.OutTime,
                                FineAmount = finePolicy.FineAmount
                            };
                            fts.Add(ft);
                        }
                    }
                }
                _applicationDbContext.FineTransactions.AddRange(fts);//Adding the record Students DBSet
                _applicationDbContext.SaveChanges();//saving the record to the database
                isSuccess= true;
            }
            catch(Exception ex) when(ex.InnerException.Message.Contains("Cannot insert duplicate key row in object")){
                TempData["msg"] = $"Attendance date {viewModel.FromDayEndDate} is already exists in system.";
                return RedirectToAction("List");
            }
            catch(Exception e) {

            }
            if(isSuccess) {
                TempData["msg"] = $"Saving Day End Process from {viewModel.FromDayEndDate.ToString("yyyyMMdd")} to {viewModel.ToDayEndDate.ToString("yyyyMMdd")}";
            }
            else
                TempData["msg"] = "Error occur when processing Day End !!";

            return RedirectToAction("List");
        }//end of entry post method   

        public IActionResult List()
        {
          IList<FineTransactionViewModel> fines= _applicationDbContext.FineTransactions.Select
                (s=>new FineTransactionViewModel
                {
                Id=s.Id,
                FinedDate=s.FinedDate,
                InTime=s.InTime,
                OutTime=s.OutTime,          
                FineAmount= s.FineAmount,
                FinePolicyId=s.FinePolicyId,
                FinePolicy= s.FinePolicy,
                StudentId = s.StudentId,
                Student =s.Student
                }).ToList();
            return View(fines);
        }

        public IActionResult Delete(string Id) {
            var ft=_applicationDbContext.FineTransactions.Find(Id);
            if (ft != null) {
                _applicationDbContext.FineTransactions.Remove(ft);//remove the  student record from DBSET
                _applicationDbContext.SaveChanges();//remove effect to the database.
                TempData["msg"] = $"Deleted successed.";
            }
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
