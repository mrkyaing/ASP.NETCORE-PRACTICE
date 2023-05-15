using SFMS.Models.ViewModels;
using System.Collections.Generic;

namespace SFMS.Services {
    public interface ICourseService {
        void Entry(CourseViewModel courseViewModel);
        IList<CourseViewModel> GetAll();
    }
}
