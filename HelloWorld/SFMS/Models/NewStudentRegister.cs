using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models {
    [Table("NewStudentRegister")]
    public class NewStudentRegister: BaseEntity {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        public string Remark { get; set; }
    }
}
