using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models.ViewModels {
    public class NewStudentRegisterViewModel {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CourseId { get; set; }
        public string Remark { get; set; }
    }
}
