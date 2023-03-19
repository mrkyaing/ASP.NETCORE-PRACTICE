using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SFMS.Models.DAO;
using SFMS.REPORT;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SFMS.Controllers {
    public class ReportController : Controller {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDbContext _applicationDbContext;
        //Constructore Inject Apporach for ApplicationDbContext;
        public ReportController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext applicationDbContext) {
            this.webHostEnvironment = webHostEnvironment;
            _applicationDbContext = applicationDbContext;
           Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        public IActionResult StudentDetail() {
            return View();
        }

        public IActionResult StudentDetailFromCodeToCode(string FromCode,string ToCode) {
            var rdlcFilePath = $"{webHostEnvironment.WebRootPath}\\ReportFiles\\StudentReport.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("rp1", $"From Code:{FromCode} To Code :{ToCode}");
            LocalReport localReport = new LocalReport(rdlcFilePath); 
            IList<StudentReport> students = _applicationDbContext.Students.Where(x=>x.Code.CompareTo(FromCode) >= 0 && x.Code.CompareTo(ToCode) <= 0).Select
                  (s => new StudentReport{
                      Code = s.Code,
                      Name = s.Name,
                      Email = s.Email,
                      Address = s.Address,
                      Phone = s.Phone,
                      FatherName = s.FatherName,
                      NRC = s.NRC,
                      DOB = s.DOB,
                  }).OrderBy(o=>o.Code).ToList();
            localReport.AddDataSource("dsStudent", students);
            var result = localReport.Execute(RenderType.Pdf, 1, parameters, null);
            return File(result.MainStream, "application/pdf");
        }
    }
}
