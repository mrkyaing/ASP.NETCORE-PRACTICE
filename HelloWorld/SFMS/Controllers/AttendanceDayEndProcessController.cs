using Microsoft.AspNetCore.Mvc;
using SFMS.Models.ViewModels;
using SFMS.Models.DAO;
using SFMS.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Collections.Generic;

namespace SFMS.Controllers{
    public class AttendanceDayEndProcessController : Controller{
        private readonly ApplicationDbContext _applicationDbContext;
        //Constructore Inject Apporach for ApplicationDbContext;
        public AttendanceDayEndProcessController(ApplicationDbContext applicationDbContext){
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult DayEndProcess() {
            IList<StudentViewModel> students = _applicationDbContext.Students.Select(b => new StudentViewModel
            {
                Name = b.Name,
                Id = b.Id,
                Code = b.Code
            }).ToList();
            IList<BatchViewModel> batches = _applicationDbContext.Batches.Select(b => new BatchViewModel
            {
                Name = b.Name,
                Id = b.Id,
                Description = b.Description
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
            //declare fts for collecting fineTransaction and then pass to database for inserting process 
            IList<FineTransaction> fts = new List<FineTransaction>();
            bool isSuccess =false;
            try {
                //getting the existing fine transaction before dayend Process by student Id and between attendance Date (from & to)
                var ftsBeforeDayEndProcess = _applicationDbContext.FineTransactions.Where(x => x.StudentId.Equals(viewModel.StudentId) 
                                            && (x.FinedDate >= viewModel.FromDayEndDate && x.FinedDate <= viewModel.ToDayEndDate)).ToList();
              
                if(ftsBeforeDayEndProcess.Count()>0) {
                    _applicationDbContext.FineTransactions.RemoveRange(ftsBeforeDayEndProcess);
                    _applicationDbContext.SaveChanges();//saving the record to the database
                }

                //start processing the fine transaction by student Id and between attendance Date (from & to)
                if (!viewModel.StudentId.Equals("so")) {
                    //get the finePolicy accroding to student by by looking up three tables (FinePolicy , Bath, Student )
                    var finePolicy = (from fp in _applicationDbContext.FinePolicies 
                                      join b in _applicationDbContext.Batches 
                                      on fp.BatchId equals b.Id
                                      join s in _applicationDbContext.Students
                                      on b.Id equals s.BathId
                                      where s.Id==viewModel.StudentId && fp.IsEnable==true select fp).SingleOrDefault();
                    var attendances = _applicationDbContext.Attendances.Where(x=>x.StudentId.Equals(viewModel.StudentId)
                    && x.IsLate==true
                    && (x.AttendaceDate>=viewModel.FromDayEndDate && x.AttendaceDate<=viewModel.ToDayEndDate)).ToList();
                    foreach(var attendance in attendances) {
                        TimeSpan inTime=TimeSpan.Parse(attendance.InTime);//change string value "13:10" to TimeSpan Type 13:10
                        //13:10 >> 13:05
                        if (inTime.Minutes > finePolicy.FineAfterMinutes) { //if your inTime is greater than Fine After Minutes,you are fine accroding to fine rules.
                            FineTransaction ft = new FineTransaction(){
                                //audit columns
                            Id = Guid.NewGuid().ToString(),
                            CreatedDte = DateTime.Now,
                            IP = GetLocalIPAddress(),//calling the method 
                               //process columns 
                                FinedDate = attendance.AttendaceDate,
                                StudentId = attendance.StudentId,
                                FinePolicyId = finePolicy.Id,
                                InTime= attendance.InTime,
                                OutTime= attendance.OutTime,
                                FineAmount=finePolicy.FineAmount
                            };
                            fts.Add(ft);//adding list of fineTransaction for each student and its attendance fine amount 
                        }
                    }//end of looping  for the whole attendance of student id 
                    _applicationDbContext.FineTransactions.AddRange(fts);//Adding the record Students DBSet
                    _applicationDbContext.SaveChanges();//saving the record to the database
                    isSuccess = true;
                }//end of if studnet id selected by ui 

                //start processing the fine transaction by bath Id and between attendance Date (from & to)
                else if (!viewModel.BathId.Equals("so")) {
                    //get the finePolicy accroding to Bath by looking up FinePolicy Table 
                    var finePolicy =_applicationDbContext.FinePolicies.Where(x=>x.BatchId.Equals(viewModel.BathId) && x.IsEnable==true).SingleOrDefault();
                    var attendances = (from a in _applicationDbContext.Attendances
                                                join s in _applicationDbContext.Students
                                                on a.StudentId equals s.Id
                                                join b in _applicationDbContext.Batches
                                                on s.BathId equals b.Id
                                                where s.BathId.Equals(viewModel.BathId) 
                                                && (a.AttendaceDate>=viewModel.FromDayEndDate && a.AttendaceDate<=viewModel.ToDayEndDate)
                                                select new AttendanceViewModel{
                                                    AttendaceDate=a.AttendaceDate,
                                                    StudentId=a.StudentId,
                                                    InTime=a.InTime,
                                                    OutTime=a.OutTime,
                                                }).ToList();
                    foreach (var item in attendances) {
                        TimeSpan inTime = TimeSpan.Parse(item.InTime);
                        if (inTime.Minutes > finePolicy.FineAfterMinutes) {
                            FineTransaction ft = new FineTransaction(){
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
                            fts.Add(ft);// adding list of fineTransaction for each student and its attendance fine amount
                        }
                    }//end of foreach attendance 
                    _applicationDbContext.FineTransactions.AddRange(fts);//Adding the record Students DBSet
                    _applicationDbContext.SaveChanges();//saving the record to the database
                    isSuccess = true;
                }//end of if bathid!="so"
            }
            catch(Exception ex){
                isSuccess = false;
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
          IList<FineTransactionViewModel> fines= _applicationDbContext.FineTransactions.Select(s=>
               new FineTransactionViewModel{
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
                _applicationDbContext.FineTransactions.Remove(ft);//remove the  record from DBSET
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
