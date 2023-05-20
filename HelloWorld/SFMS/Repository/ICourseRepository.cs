using SFMS.Models;
using System.Collections.Generic;

namespace SFMS.Repository {
    public interface ICourseRepository {
         void Create(Course model);
         IEnumerable<Course> ReteriveActive();
        void Update(Course model);
        void Delete(string id);
        Course FindById(string id);
    }
}
