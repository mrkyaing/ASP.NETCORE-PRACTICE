using Microsoft.AspNetCore.Mvc;
using SFMS.Models.DAO;
using System.Linq;
using System.Threading.Tasks;

namespace SFMS.ViewComponents {
    [ViewComponent(Name = "AvailableCoursesNav")]
    public class AllAvailableCoursesNav: ViewComponent {
        private readonly ApplicationDbContext _context;
        public AllAvailableCoursesNav(ApplicationDbContext applicationDbContext) {
            _context = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var AllCourses = _context.Courses.ToList();
            return View("AllAvailableCoursesNav", AllCourses);
        }
    }
}
