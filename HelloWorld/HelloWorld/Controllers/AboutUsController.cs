using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Web;

namespace HelloWorld.Controllers
{
    public class AboutUsController :Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
       
        //constructor inject for IWebHostEnvironment interface .
        public AboutUsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Hi()
        {
           return View();
        }
       // [ActionName("Me")]
        public ViewResult Me(string firstName,string lastName)
        {
            ViewBag.myName =$"{firstName} {lastName}" ;
            return View();
        }
        [NonAction]
        public string Ok() => "ok";
   

        public ContentResult ShowContent()
        {
            return Content ("<h1>I am C# Developer <h1>","text/html");
        }
        [Authorize]
        public FileResult DownloadApp()
        {
            string fileName = "UserManual.txt";
            //create the file path under wwwroot/files/{yourfile}
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Files\\") + fileName;
            //read the file according to path and then store as byte[] 
            byte[] fileInBytes=System.IO.File.ReadAllBytes(path);
            //response file with byte records and its name.
            return File(fileInBytes, "text/plain",fileName);
        }
        //url pattern call at browser >>
        //http://localhost:62383/aboutus/calculate?n1=20&n2=20
        public ViewResult Calculate(int n1,int n2)
        {
            ViewBag.Result = n1 + n2;
            return View();
        }
        public FileResult DownloadFile()
        {
            string fileName = "UserManual.txt";
            byte[] myFile = System.IO.File.ReadAllBytes("Files/"+fileName);
            return File(myFile, "text/txt", fileName);
        }
        [HttpPost]
        public IActionResult Sum(int n1,int n2)
        {
            ViewBag.Result = n1 + n2;
            return View();
        }

        public  ViewResult Testing()
        {
            ViewData["FullName"] = "John Smith";
            return View();
        }
    }
}
