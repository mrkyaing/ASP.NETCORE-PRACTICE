using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models { 
    [Table("Batch")]
    public class Batch:BaseEntity{       
        public string Name { get; set; }
        public string Description { get; set; }
        public string CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        public virtual IList<Student> Students { get; set; }
    }
}
