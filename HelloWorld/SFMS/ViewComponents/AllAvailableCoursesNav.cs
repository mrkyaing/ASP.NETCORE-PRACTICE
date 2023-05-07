using Microsoft.AspNetCore.Mvc;
using SFMS.Models.DAO;
using System.Linq;

namespace SFMS.ViewComponents {
    [ViewComponent(Name = "AvailableCoursesNav")]
    public class AllAvailableCoursesNav: ViewComponent {
        private readonly ApplicationDbContext _applicationDbcontext;
        public AllAvailableCoursesNav(ApplicationDbContext applicationDbContext) {
            _applicationDbcontext = applicationDbContext;
        }

        public IViewComponentResult Invoke() {
            var AllCourses = _applicationDbcontext.Courses.ToList();
            return View("AllAvailableCoursesNav", AllCourses);
        }
    }
}
