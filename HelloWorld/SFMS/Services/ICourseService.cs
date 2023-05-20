using SFMS.Models.ViewModels;
using System.Collections.Generic;

namespace SFMS.Services {
    public interface ICourseService {
        void Create(CourseViewModel courseViewModel);
        IList<CourseViewModel> ReteriveActive();
        void  Update(CourseViewModel courseViewModel);
        void Delete(string id);
        CourseViewModel FindById(string id);
    }
}
