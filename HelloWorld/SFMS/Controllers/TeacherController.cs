using Microsoft.AspNetCore.Mvc;
using SFMS.Models.DAO;

namespace SFMS.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TeacherController(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
