using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models {
    [Table("TeacherCourses")]
    public class TeacherCourses:BaseEntity {
        public string TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }
        public string CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual IList<Course> Courses { get; set; }
    }
}
